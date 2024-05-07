using TechLanches.Adapter.RabbitMq;
using TechLanches.Adapter.RabbitMq.Messaging;
using TechLanches.Application.Controllers.Interfaces;
using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways;
using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Application.Ports.Repositories;
using TechLanches.Application.Presenters.Interfaces;
using TechLanches.Application.UseCases.Pedidos;
using TechLanches.Domain.Enums;
using TechLanches.Domain.Services;
using TechLanches.Domain.ValueObjects;

namespace TechLanches.Application.Controllers
{
    public class PedidoController : IPedidoController
    {
        private readonly IPedidoGateway _pedidoGateway;
        private readonly IPagamentoGateway _pagamentoGateway;
        private readonly IPedidoPresenter _pedidoPresenter;
        private readonly IStatusPedidoValidacaoService _statusPedidoValidacaoService;
        private readonly IRabbitMqService _rabbitmqService;

        public PedidoController(
            IPedidoRepository pedidoRepository,
            IPedidoPresenter pedidoPresenter,
            IStatusPedidoValidacaoService statusPedidoValidacaoService,
            IRabbitMqService rabbitmqService,
            IPagamentoGateway pagamentoGateway)
        {
            _pedidoGateway = new PedidoGateway(pedidoRepository);
            _pedidoPresenter = pedidoPresenter;
            _statusPedidoValidacaoService = statusPedidoValidacaoService;
            _rabbitmqService = rabbitmqService;
            _pagamentoGateway = pagamentoGateway;
        }

        public async Task<List<PedidoResponseDTO>> BuscarTodos()
        {
            var pedidos = await _pedidoGateway.BuscarTodos();

            return await _pedidoPresenter.ParaListaDto(pedidos, _pagamentoGateway);
        }

        public async Task<PedidoResponseDTO> BuscarPorId(int idPedido)
        {
            var pedido = await _pedidoGateway.BuscarPorId(idPedido);

            return await _pedidoPresenter.ParaDto(pedido, _pagamentoGateway);
        }

        public async Task<List<PedidoResponseDTO>> BuscarPorStatus(StatusPedido statusPedido)
        {
            var pedidos = await _pedidoGateway.BuscarPorStatus(statusPedido);

            return await _pedidoPresenter.ParaListaDto(pedidos, _pagamentoGateway);
        }

        public async Task<PedidoResponseDTO> Cadastrar(Cpf cpf, List<ItemPedido> itensPedido)
        {
            var pedido = await PedidoUseCases.Cadastrar(cpf, itensPedido, _pedidoGateway);
            await _pedidoGateway.CommitAsync();

            return await _pedidoPresenter.ParaDto(pedido, _pagamentoGateway);
        }

        public async Task<PedidoResponseDTO> TrocarStatus(int pedidoId, StatusPedido statusPedido)
        {
            var pedido = await PedidoUseCases.TrocarStatus(pedidoId, statusPedido, _pedidoGateway, _statusPedidoValidacaoService);
            
            await _pedidoGateway.CommitAsync();

            if (statusPedido == StatusPedido.PedidoRecebido)
            {
                var message = new PedidoMessage(pedido.Id, pedido.Cpf.Numero);
                _rabbitmqService.Publicar(message);
            }

            return await _pedidoPresenter.ParaDto(pedido, _pagamentoGateway);
        }
    }
}
