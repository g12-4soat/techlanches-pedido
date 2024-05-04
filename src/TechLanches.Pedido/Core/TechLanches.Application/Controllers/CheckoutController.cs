using TechLanches.Application.Controllers.Interfaces;
using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways;
using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Application.Ports.Repositories;
using TechLanches.Application.UseCases.Pagamentos;

namespace TechLanches.Application.Controllers
{
    public class CheckoutController : ICheckoutController
    {
        private readonly IPedidoGateway _pedidoGateway;
        private readonly IPagamentoGateway _pagamentoGateway;

        public CheckoutController(
            IPedidoRepository pedidoRepository,
            IPagamentoGateway pagamentoGateway)
        {
            _pedidoGateway = new PedidoGateway(pedidoRepository);
            _pagamentoGateway = pagamentoGateway;
        }

        public async Task<bool> ValidarCheckout(int pedidoId)
            => await CheckoutUseCase.ValidarPedidoCompleto(pedidoId, _pedidoGateway, _pagamentoGateway);

        public async Task<PagamentoResponseDTO> GerarPagamentoCheckout(int pedidoId, decimal valor)
        {
            var dto = new PagamentoRequestDTO { PedidoId = pedidoId, Valor = valor };

            return await _pagamentoGateway.GerarPagamento(dto);
        }
    }
}