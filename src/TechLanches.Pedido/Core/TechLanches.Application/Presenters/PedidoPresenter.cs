using Mapster;
using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Application.Presenters.Interfaces;
using TechLanches.Domain.Aggregates;

namespace TechLanches.Application.Presenters
{
    public class PedidoPresenter : IPedidoPresenter
    {
        public PedidoResponseDTO ParaDto(Pedido entidade)
        {
            return entidade.Adapt<PedidoResponseDTO>();
        }

        public async Task<PedidoResponseDTO> ParaDto(Pedido entidade, IPagamentoGateway pagamentoGateway)
        {
            PedidoResponseDTO dto = await PreencherPedido(entidade, pagamentoGateway);
            return dto;
        }

        public async Task<List<PedidoResponseDTO>> ParaListaDto(List<Pedido> entidades, IPagamentoGateway pagamentoGateway)
        {
            var lista = new List<PedidoResponseDTO>();

            foreach (var entidade in entidades)
            {
                PedidoResponseDTO dto = await PreencherPedido(entidade, pagamentoGateway);
                lista.Add(dto);
            }

            return lista;
        }

        private static async Task<PedidoResponseDTO> PreencherPedido(Pedido entidade, IPagamentoGateway pagamentoGateway)
        {
            //framework de mapeamento não consegue lidar com value objects
            var dto = entidade.Adapt<PedidoResponseDTO>();
            dto.ClienteCpf = entidade.Cpf.Numero;
            dto.Pagamento = await pagamentoGateway.RetornarPagamentoPorPedidoId(entidade.Id);
            return dto;
        }
    }
}
