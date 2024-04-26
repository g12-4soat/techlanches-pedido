using TechLanches.Application.DTOs;

namespace TechLanches.Application.Gateways.Interfaces
{
    public interface IPagamentoGateway
    {
        Task<PagamentoResponseDTO> RetornarPagamentoPorPedidoId(int pedidoId);
        Task<CheckoutResponseDTO> GerarPagamento(int pedidoId);
    }
}
