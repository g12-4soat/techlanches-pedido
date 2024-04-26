using System;
using TechLanches.Domain.Constantes;
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
            var cpf = RetornarCpf(user);
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

        private static Cpf RetornarCpf(UserTokenDTO user)
        {
            var cpf = user.Username == Constants.USER_DEFAULT ? Constants.CPF_USER_DEFAULT : user.Username;

            return new Cpf(cpf);
        }
    }
}
