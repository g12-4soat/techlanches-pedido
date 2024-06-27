using Mapster;
using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Application.Presenters.Interfaces;
using TechLanches.Domain.Aggregates;
using TechLanches.Domain.Constantes;

namespace TechLanches.Application.Presenters
{
    public class PedidoPresenter : IPedidoPresenter
    {
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

        public PedidoResponseDTO ParaDto(Pedido pedido)
        {
            var dto = pedido.Adapt<PedidoResponseDTO>();
            dto.ClienteCpf = RetornarCpfCliente(pedido);
            return dto;
        }

        private static async Task<PedidoResponseDTO> PreencherPedido(Pedido entidade, IPagamentoGateway pagamentoGateway)
        {
            var dto = entidade.Adapt<PedidoResponseDTO>();
            dto.ClienteCpf = RetornarCpfCliente(entidade);
            dto.Pagamento = await pagamentoGateway.RetornarPagamentoPorPedidoId(entidade.Id);
            return dto;
        }

        private static string RetornarCpfCliente(Pedido pedido)
        {
            return ClienteNaoIdentificavel(pedido)
               ? Constants.USER_NAO_IDENTIFICADO            
               : pedido.Cpf.Numero; //framework de mapeamento não consegue lidar com value objects

            static bool ClienteNaoIdentificavel(Pedido pedido)
            {
                return pedido.Cpf.Numero == Constants.CPF_USER_DEFAULT
                    || pedido.ClienteInativo;
            }
        }
    }
}
