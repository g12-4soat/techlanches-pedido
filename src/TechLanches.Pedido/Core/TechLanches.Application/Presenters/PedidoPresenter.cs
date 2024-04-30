using Mapster;
using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Application.Presenters.Interfaces;
using TechLanches.Domain.Aggregates;

namespace TechLanches.Application.Presenters
{
    public class PedidoPresenter : IPedidoPresenter
    {
        public async Task<PedidoResponseDTO> ParaDto(Pedido entidade, IPagamentoGateway pagamentoGateway)
        {
            var dto = entidade.Adapt<PedidoResponseDTO>();
            dto.Pagamento = await pagamentoGateway.RetornarPagamentoPorPedidoId(entidade.Id);
            return dto;
        }

        public async Task<List<PedidoResponseDTO>> ParaListaDto(List<Pedido> entidades, IPagamentoGateway pagamentoGateway)
        {
            var lista = new List<PedidoResponseDTO>();

            foreach (var entidade in entidades)
            {
                var dto = entidade.Adapt<PedidoResponseDTO>();
                dto.Pagamento = await pagamentoGateway.RetornarPagamentoPorPedidoId(entidade.Id);
                lista.Add(dto);
            }

            return lista;
        }
    }
}
