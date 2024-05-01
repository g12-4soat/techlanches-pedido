﻿using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Core;
using TechLanches.Domain.Enums;

namespace TechLanches.Application.UseCases.Pagamentos
{
    public class CheckoutUseCase
    {
        public static async Task<bool> ValidarPedidoCompleto(
            int pedidoId,
            IPedidoGateway pedidoGateway,
            IPagamentoGateway pagamentoGateway)
        {
            var pedido = await pedidoGateway.BuscarPorId(pedidoId)
                ?? throw new DomainException($"Pedido não encontrado para checkout - PedidoId: {pedidoId}");

            if (pedido.StatusPedido != StatusPedido.PedidoCriado)
                throw new DomainException($"Status não autorizado para checkout - StatusPedido: {pedido.StatusPedido}");

            if (await VerificarSeExistemPagamentos(pedidoId, pagamentoGateway))
                throw new DomainException($"Pedido já contém pagamento");

            return true;
        }

        private async static Task<bool> VerificarSeExistemPagamentos(int pedidoId, IPagamentoGateway pagamentoGateway)
        {
            var pagamento = await pagamentoGateway.RetornarPagamentoPorPedidoId(pedidoId);

            return pagamento is not null && pagamento.PedidoId != 0;
        }
    }
}
