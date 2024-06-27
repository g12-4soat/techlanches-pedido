using NSubstitute.ReturnsExtensions;
using TechLanches.Application.Gateways.Interfaces;
using TechLanchesPedido.Tests.Fixtures;

namespace TechLanchesPedido.Tests.BDDTests.Services
{
    [Trait("Services", "Checkout")]
    public class CheckoutTest : IClassFixture<PedidoFixture>
    {
        private bool _resultado;
        private Pedido _pedido;
        private PagamentoResponseDTO _pagamentoResponseDto;
        private IPagamentoGateway _pagamentoGateway;
        private IPedidoRepository _pedidoRepository;

        private readonly PedidoFixture _pedidoFixture;

        public CheckoutTest(PedidoFixture pedidoFixture)
        {
            _pedidoFixture = pedidoFixture;
        }

        [Fact]
        public async Task ValidarCheckout_PedidoEPagamentoValido()
        {
            Given_PedidoValido();
            Given_PagamentoInexistente();
            await When_ValidarCheckout();
            Then_DeveRetornarSucesso();
        }

        [Fact]
        public async Task ValidarCheckout_PedidoEPagamentoInvalido()
        {
            Given_PedidoInvalido();
            Given_PagamentoExistente();
            await When_ValidarCheckout();
            Then_DeveRetornarFalha();
        }

        [Fact]
        public async Task GerarPagamentoCheckout_DeveRetornarSucesso()
        {
            Given_PedidoValido();
            Given_PagamentoGatewayValido();
            await When_GerarPagamento();
            Then_DeveRetornarPagamentoResponseDto();
        }

        #region Given

        private void Given_PagamentoGatewayValido()
        {
            _pagamentoGateway = Substitute.For<IPagamentoGateway>();
        }

        private void Given_PedidoValido()
        {
            _pedidoRepository = Substitute.For<IPedidoRepository>();
            _pedido = _pedidoFixture.GerarPedidoValido();
            _pedidoRepository.BuscarPorId(_pedido.Id).Returns(_pedido);
        }

        private void Given_PagamentoInexistente()
        {
            _pagamentoGateway = Substitute.For<IPagamentoGateway>();
            _pagamentoGateway.RetornarPagamentoPorPedidoId(_pedido.Id).ReturnsNull();
        }

        private void Given_PagamentoExistente()
        {
            PagamentoResponseDTO pagamento = GerarPagamentoResponseDtoValido();

            _pagamentoGateway = Substitute.For<IPagamentoGateway>();
            _pagamentoGateway.RetornarPagamentoPorPedidoId(Arg.Any<int>()).Returns(pagamento);
        }

        private void Given_PedidoInvalido()
        {
            _pedidoRepository = Substitute.For<IPedidoRepository>();
            _pedidoRepository.BuscarPorId(Arg.Any<int>()).ReturnsNull();
        }

        #endregion

        #region When

        private async Task When_GerarPagamento()
        {
            _pagamentoResponseDto = GerarPagamentoResponseDtoValido();
            _pagamentoGateway.GerarPagamento(Arg.Any<PagamentoRequestDTO>()).Returns(_pagamentoResponseDto);
            var checkoutController = new CheckoutController(_pedidoRepository, _pagamentoGateway);

            _pagamentoResponseDto = await checkoutController.GerarPagamentoCheckout(_pedido.Id, _pedido.Valor);
        }

        private async Task When_ValidarCheckout()
        {
            var checkoutController = new CheckoutController(_pedidoRepository, _pagamentoGateway);

            _resultado = await checkoutController.ValidarCheckout(_pedido?.Id ?? Arg.Any<int>());
        }

        #endregion

        #region Then

        private void Then_DeveRetornarSucesso()
        {
            Assert.True(_resultado);
        }

        private void Then_DeveRetornarFalha()
        {
            Assert.False(_resultado);
        }

        private void Then_DeveRetornarPagamentoResponseDto()
        {
            Assert.NotNull(_pagamentoResponseDto);
        }

        #endregion

        private static PagamentoResponseDTO GerarPagamentoResponseDtoValido()
        {
            return new PagamentoResponseDTO()
            {
                PedidoId = 1,
                QRCodeData = string.Empty,
                Valor = (decimal)new Random().NextDouble()
            };
        }
    }
}
