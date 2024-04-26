using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways.Interfaces;

namespace TechLanches.Application.Gateways
{
    public class PagamentoGateway : IPagamentoGateway
    {
        public Task<CheckoutResponseDTO> GerarPagamento(int pedidoId)
        {
            throw new NotImplementedException();
        }

        public Task<PagamentoResponseDTO> RetornarPagamentoPorPedidoId(int pedidoId)
        {
            throw new NotImplementedException();
        }
    }
}
