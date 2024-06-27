using TechLanches.Application.Gateways.Interfaces;
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
            var pedido = await pedidoGateway.BuscarPorId(pedidoId);

            //var result = pedido is not null
            //    && pedido.StatusPedido == StatusPedido.PedidoCriado
            //    && !await VerificarSeExistemPagamentos(pedidoId, pagamentoGateway);

            var result = pedido is not null
                && pedido.StatusPedido == StatusPedido.PedidoCriado;

            return result;
        }

        private async static Task<bool> VerificarSeExistemPagamentos(int pedidoId, IPagamentoGateway pagamentoGateway)
        {
            var pagamento = await pagamentoGateway.RetornarPagamentoPorPedidoId(pedidoId);

            return pagamento is not null && pagamento.PedidoId != 0;
        }
    }
}
