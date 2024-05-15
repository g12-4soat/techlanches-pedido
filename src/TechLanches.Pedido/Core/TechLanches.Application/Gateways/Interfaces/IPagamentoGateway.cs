using TechLanches.Application.DTOs;

namespace TechLanches.Application.Gateways.Interfaces
{
    public interface IPagamentoGateway
    {
        void AddAuthenticationHeader(string key);
        Task<PagamentoResponseDTO> RetornarPagamentoPorPedidoId(int pedidoId);
        Task<PagamentoResponseDTO> GerarPagamento(PagamentoRequestDTO pagamentoRequest);
    }
}
