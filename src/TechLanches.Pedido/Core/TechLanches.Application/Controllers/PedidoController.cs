using Microsoft.Extensions.Options;
using System.Transactions;
using TechLanches.Adapter.RabbitMq;
using TechLanches.Adapter.RabbitMq.Messaging;
using TechLanches.Adapter.RabbitMq.Options;
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
        private readonly RabbitOptions _rabbitOptions;

        public PedidoController(
            IPedidoRepository pedidoRepository,
            IPedidoPresenter pedidoPresenter,
            IStatusPedidoValidacaoService statusPedidoValidacaoService,
            IRabbitMqService rabbitmqService,
            IPagamentoGateway pagamentoGateway,
           IOptions<RabbitOptions> rabbitOptions)
        {
            _pedidoGateway = new PedidoGateway(pedidoRepository);
            _pedidoPresenter = pedidoPresenter;
            _statusPedidoValidacaoService = statusPedidoValidacaoService;
            _rabbitmqService = rabbitmqService;
            _pagamentoGateway = pagamentoGateway;
            _rabbitOptions = rabbitOptions.Value;
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
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var pedido = await PedidoUseCases.Cadastrar(cpf, itensPedido, _pedidoGateway);
                await _pedidoGateway.CommitAsync();

                var message = new PedidoCriadoMessage(pedido.Id, pedido.Valor);
                _rabbitmqService.Publicar(message, _rabbitOptions.QueueOrderCreated);

                scope.Complete();
                return _pedidoPresenter.ParaDto(pedido);
            }
        }

        public async Task<PedidoResponseDTO> TrocarStatus(int pedidoId, StatusPedido statusPedido)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var pedido = await PedidoUseCases.TrocarStatus(pedidoId, statusPedido, _pedidoGateway, _statusPedidoValidacaoService);

                await _pedidoGateway.CommitAsync();

                if (statusPedido == StatusPedido.PedidoRecebido)
                {
                    var message = new PedidoMessage(pedido.Id, pedido.Cpf.Numero);
                    _rabbitmqService.Publicar(message, _rabbitOptions.Queue);
                }

                scope.Complete();
                return await _pedidoPresenter.ParaDto(pedido, _pagamentoGateway);
            }
        }

        public async Task ProcessarMensagem(PedidoStatusMessage message)
        {
            await TrocarStatus(message.PedidoId, message.StatusPedido);
        }
    }
}
