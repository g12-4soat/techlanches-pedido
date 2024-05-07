using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Core;
using TechLanches.Domain.Aggregates;
using TechLanches.Domain.Enums;
using TechLanches.Domain.Services;
using TechLanches.Domain.ValueObjects;

namespace TechLanches.Application.UseCases.Pedidos
{
    public class PedidoUseCases
    {
        public static async Task<Pedido> Cadastrar(Cpf cpf, List<ItemPedido> itensPedido, IPedidoGateway pedidoGateway)
        {
            var pedido = new Pedido(cpf, itensPedido);

            pedido = await pedidoGateway.Cadastrar(pedido);
            return pedido;
        }

        public static async Task<Pedido> TrocarStatus(
            int pedidoId, 
            StatusPedido statusPedido, 
            IPedidoGateway pedidoGateway, 
            IStatusPedidoValidacaoService statusPedidoValidacaoService)
        {
            var pedido = await pedidoGateway.BuscarPorId(pedidoId)
               ?? throw new DomainException("Não foi encontrado nenhum pedido com id informado.");

            pedido.TrocarStatus(statusPedidoValidacaoService, statusPedido);

            return pedido;
        }
    }
}
