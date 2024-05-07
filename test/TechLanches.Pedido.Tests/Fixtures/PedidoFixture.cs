using TechLanches.Domain.Services;
using TechLanches.Domain.Validations;

namespace TechLanchesPedido.Tests.Fixtures
{
    public class PedidoFixture : IDisposable
    {
        public IStatusPedidoValidacaoService? StatusPedidoValidacaoService;

        public PedidoFixture()
        {
            var validacoes = new List<IStatusPedidoValidacao>
            {
                new StatusPedidoCriadoValidacao(),
                new StatusPedidoCanceladoValidacao(),
                new StatusPedidoCanceladoPorPagamentoValidacao(),
                new StatusPedidoEmPreparacaoValidacao(),
                new StatusPedidoDescartadoValidacao(),
                new StatusPedidoFinalizadoValidacao(),
                new StatusPedidoProntoValidacao(),
                new StatusPedidoRecebidoValidacao(),
                new StatusPedidoRetiradoValidacao()
            };

            StatusPedidoValidacaoService = new StatusPedidoValidacaoService(validacoes);
        }

        public Pedido GerarPedidoValido()
        {
            return new Pedido(new Cpf(Constants.CPF_USER_DEFAULT), GerarItensPedidoValidos());
        }

        public List<Pedido> GerarPedidosValidos()
        {
            return new List<Pedido> { GerarPedidoValido() };
        }

        public ItemPedido GerarItemPedidoValido()
        {
            return new ItemPedido(1, 1, 1);
        }

        public List<ItemPedido> GerarItensPedidoValidos()
        {
            return new List<ItemPedido> { GerarItemPedidoValido() };
        }

        public void Dispose()
        {
            StatusPedidoValidacaoService = null;
        }
    }
}
