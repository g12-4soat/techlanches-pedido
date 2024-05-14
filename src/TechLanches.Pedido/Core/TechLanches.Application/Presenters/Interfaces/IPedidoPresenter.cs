using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Domain.Aggregates;

namespace TechLanches.Application.Presenters.Interfaces
{
    public interface IPedidoPresenter
    {
        PedidoResponseDTO ParaDto(Pedido pedido);
        Task<PedidoResponseDTO> ParaDto(Pedido entidade, IPagamentoGateway pagamentoGateway);
        Task<List<PedidoResponseDTO>> ParaListaDto(List<Pedido> entidades, IPagamentoGateway pagamentoGateway);
    }
}
