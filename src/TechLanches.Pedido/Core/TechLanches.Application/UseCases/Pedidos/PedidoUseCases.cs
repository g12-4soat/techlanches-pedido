using TechLanches.Application.DTOs;
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
        public static async Task<Pedido> Cadastrar(UserTokenDTO user, List<ItemPedido> itensPedido, IPedidoGateway pedidoGateway)
        {
            //validar no cognito?
            //var cliente = await ClienteUseCases.IdentificarCliente(user);
            //TODO trocar pedido para usar cpf
            var clienteId = user.Username == "usertechlanches" ? 0 : Convert.ToInt32(user.Username);
            var pedido = new Pedido(clienteId, itensPedido);

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
