using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways.Interfaces;

namespace TechLanches.Application.Gateways
{
    public class PagamentoGateway : IPagamentoGateway
    {
        public Task<CheckoutResponseDTO> GerarPagamento(int pedidoId)
        {
            return Task.FromResult(new CheckoutResponseDTO { PedidoId = pedidoId });
        }

        public Task<PagamentoResponseDTO> RetornarPagamentoPorPedidoId(int pedidoId)
        {
            return Task.FromResult(new PagamentoResponseDTO());
        }
    }
}
