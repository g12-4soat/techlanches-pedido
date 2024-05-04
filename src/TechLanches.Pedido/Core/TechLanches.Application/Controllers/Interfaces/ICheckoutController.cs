using TechLanches.Application.DTOs;

namespace TechLanches.Application.Controllers.Interfaces
{
    public interface ICheckoutController
    {
        Task<bool> ValidarCheckout(int pedidoId);
        Task<PagamentoResponseDTO> GerarPagamentoCheckout(int pedidoId, decimal valor);
    }
}
