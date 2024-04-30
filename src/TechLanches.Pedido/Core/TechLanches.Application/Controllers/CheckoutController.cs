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

        public async Task<CheckoutResponseDTO> GerarPagamentoCheckout(int pedidoId)
        {            
            return await _pagamentoGateway.GerarPagamento(pedidoId);
        }
    }
}