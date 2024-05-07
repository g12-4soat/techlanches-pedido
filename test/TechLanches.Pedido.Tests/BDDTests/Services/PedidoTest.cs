using System;
using TechLanches.Adapter.RabbitMq;
using TechLanches.Adapter.RabbitMq.Messaging;
using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Domain.Aggregates;
using TechLanchesPedido.Tests.Fixtures;

namespace TechLanchesPedido.Tests.BDDTests.Services
{
    [Trait("Services", "Pedido")]
    public class PedidoTest : IClassFixture<PedidoFixture>
    {
        private Pedido _pedido;
        private List<Pedido> _pedidos;
        private Cpf _cpf;
        private List<ItemPedido> _itensPedidos;
        private PedidoResponseDTO _pedidoResponseDto;
        private List<PedidoResponseDTO> _pedidosResponseDto;
        private IPagamentoGateway _pagamentoGateway;
        private IPedidoRepository _pedidoRepository;
        private IRabbitMqService _rabbitMqService;
        private readonly PedidoFixture _pedidoFixture;
        private DomainException _domainException;

        public PedidoTest(PedidoFixture pedidoFixture)
        {
            _pedidoFixture = pedidoFixture;
        }

        [Fact(DisplayName = "Buscar todos pedidos com sucesso")]
        public async Task BuscarTodos_DeveRetornarTodosPedidos()
        {
            Given_PedidosValidos();
            await When_BuscarTodos();
            Then_DeveRetornarListaDePedidosNaoVazia();
            await _pedidoRepository.Received().BuscarTodos();
        }

        [Fact(DisplayName = "Buscar pedido por id com sucesso")]
        public async Task BuscarPorId_DeveRetornarPedidoSolicitado()
        {
            Given_PedidoValido();
            await When_BuscarPorId();
            Then_PedidoResponseDtoNaoDeveSerNulo();
            await _pedidoRepository.Received().BuscarPorId(Arg.Any<int>());
        }

        [Fact(DisplayName = "Buscar pedidos por status com sucesso")]
        public async Task BuscarPorStatus_DeveRetornarPedidoComStatusSolicitado()
        {
            Given_PedidosValidos();
            await When_BuscarPorStatus();
            Then_DeveRetornarListaDePedidosNaoVazia();            
            await _pedidoRepository.Received().BuscarPorStatus(StatusPedido.PedidoEmPreparacao);
        }

        [Fact(DisplayName = "Deve trocar status com sucesso")]
        public async Task TrocarStatus_ComStatusValido_DeveRetornarSucesso()
        {
            Given_PedidoValido();
            await When_TrocarStatusComPedidoValido();
            Then_StatusPedidoResponseDtoDeveSerIgualPedidoRecebido();
            Then_PedidoResponseDtoNaoDeveSerNulo();
            await _pedidoRepository.Received().BuscarPorId(Arg.Any<int>());
        }

        [Fact(DisplayName = "Trocar status pedido inexistente com falha")]
        public async Task TrocarStatus_ComPedidoInexistente_DeveLancarException()
        {
            Given_PedidoInexistente();
            await When_TrocarStatusComPedidoNulo();
            Then_DeveLancarDomainException();
            Then_ExceptionDeveConterMensagem("Não foi encontrado nenhum pedido com id informado.");
        }

        [Fact(DisplayName = "Trocar status pedido com falha")]
        public async Task TrocarStatus_ComStatusInvalido_DeveLancarException()
        {
            Given_PedidoValido();
            await When_TrocarStatusComStatusInvalido();
            Then_DeveLancarDomainException();
            Then_ExceptionDeveConterMensagem("O status selecionado não é válido");
        }

        [Fact(DisplayName = "Deve cadastrar pedido com sucesso")]
        public async Task CadastrarPedido_DeveRetornarSucesso()
        {
            Given_PedidoValidoComCpfEItensValidos();
            await When_CadastrarPedido();
            Then_PedidoResponseDtoNaoDeveSerNulo();
            Then_PedidoResponseCpfDeveTerMesmoCpfDoPedido();
            await _pedidoRepository.Received().Cadastrar(_pedido);
        }

        [Fact(DisplayName = "Trocar status pedido sucesso publica no RabbitMQ")]
        public async Task TrocarStatus_DevePublicarMensagemAposCommitAsync()
        {
            Given_PedidoValido();
            await When_TrocarStatusComPedidoValido();
            Then_PedidoResponseDtoNaoDeveSerNulo();
            Then_StatusPedidoResponseDtoDeveSerIgualPedidoRecebido();
            Then_DevePublicarMensagemTrocaStatus();
        }

        #region Given

        private void Given_PedidoValidoComCpfEItensValidos()
        {
            _cpf = new Cpf("046.047.173-20");
            _itensPedidos = _pedidoFixture.GerarItensPedidoValidos();
            _pedido = new Pedido(_cpf, _itensPedidos);
        }

        private void Given_PedidoValido()
        {
            _pedido = _pedidoFixture.GerarPedidoValido();
        }

        private void Given_PedidosValidos()
        {
            _pedidos = _pedidoFixture.GerarPedidosValidos();
        }

        private static void Given_PedidoInexistente()
        {

        }

        #endregion

        #region When

        private async Task When_CadastrarPedido()
        {
            _pedidoRepository = Substitute.For<IPedidoRepository>();
            _pagamentoGateway = Substitute.For<IPagamentoGateway>();
            _rabbitMqService = Substitute.For<IRabbitMqService>();

            _pedidoRepository.Cadastrar(_pedido).Returns(_pedido);

            var pedidoController = new PedidoController(
                _pedidoRepository,
                new PedidoPresenter(),
                _pedidoFixture.StatusPedidoValidacaoService,
                _rabbitMqService,
                _pagamentoGateway);

            _pedidoResponseDto = await pedidoController.Cadastrar(_cpf, _itensPedidos);
        }

        private async Task When_TrocarStatusComPedidoValido()
        {
            _pedidoRepository = Substitute.For<IPedidoRepository>();
            _rabbitMqService = Substitute.For<IRabbitMqService>();
            _pagamentoGateway = Substitute.For<IPagamentoGateway>();

            _pedidoRepository.BuscarPorId(_pedido.Id).Returns(_pedido);

            var pedidoController = new PedidoController(
                _pedidoRepository,
                new PedidoPresenter(),
                _pedidoFixture.StatusPedidoValidacaoService!,
                _rabbitMqService,
                _pagamentoGateway);

            _pedidoResponseDto = await pedidoController.TrocarStatus(_pedido.Id, StatusPedido.PedidoRecebido);
        }

        private async Task When_TrocarStatusComStatusInvalido()
        {
            _pedidoRepository = Substitute.For<IPedidoRepository>();
            _rabbitMqService = Substitute.For<IRabbitMqService>();
            _pagamentoGateway = Substitute.For<IPagamentoGateway>();

            _pedidoRepository.BuscarPorId(_pedido.Id).Returns(_pedido);

            var pedidoController = new PedidoController(
                _pedidoRepository,
                new PedidoPresenter(),
                _pedidoFixture.StatusPedidoValidacaoService!,
                _rabbitMqService,
                _pagamentoGateway);

            _domainException = await Assert.ThrowsAsync<DomainException>(async () 
                => await pedidoController.TrocarStatus(_pedido.Id, StatusPedido.PedidoFinalizado));
        }

        private async Task When_TrocarStatusComPedidoNulo()
        {
            _pedidoRepository = Substitute.For<IPedidoRepository>();
            _rabbitMqService = Substitute.For<IRabbitMqService>();
            _pagamentoGateway = Substitute.For<IPagamentoGateway>();

            _pedidoRepository.BuscarPorId(Arg.Any<int>()).Returns(_pedido);

            var pedidoController = new PedidoController(
                _pedidoRepository,
                new PedidoPresenter(),
                _pedidoFixture.StatusPedidoValidacaoService!,
                _rabbitMqService,
                _pagamentoGateway);

            _domainException = await Assert.ThrowsAsync<DomainException>(async ()
                => await pedidoController.TrocarStatus(Arg.Any<int>(), StatusPedido.PedidoFinalizado));
        }

        private async Task When_BuscarPorStatus()
        {
            _pedidoRepository = Substitute.For<IPedidoRepository>();
            _pagamentoGateway = Substitute.For<IPagamentoGateway>();
            _rabbitMqService = Substitute.For<IRabbitMqService>();

            _pedidoRepository.BuscarPorStatus(StatusPedido.PedidoEmPreparacao).Returns(_pedidos);

            var pedidoController = new PedidoController(
                _pedidoRepository,
                new PedidoPresenter(),
                _pedidoFixture.StatusPedidoValidacaoService!,
                _rabbitMqService,
                _pagamentoGateway);

            _pedidosResponseDto = await pedidoController.BuscarPorStatus(StatusPedido.PedidoEmPreparacao);
        }

        private async Task When_BuscarPorId()
        {
            _pedidoRepository = Substitute.For<IPedidoRepository>();
            _pagamentoGateway = Substitute.For<IPagamentoGateway>();
            _rabbitMqService = Substitute.For<IRabbitMqService>();

            _pedidoRepository.BuscarPorId(_pedido.Id).Returns(_pedido);

            var pedidoController = new PedidoController(
                _pedidoRepository,
                new PedidoPresenter(),
                _pedidoFixture.StatusPedidoValidacaoService!,
                _rabbitMqService,
                _pagamentoGateway);

            _pedidoResponseDto = await pedidoController.BuscarPorId(_pedido.Id);
        }

        private async Task When_BuscarTodos()
        {
            _pedidoRepository = Substitute.For<IPedidoRepository>();
            _rabbitMqService = Substitute.For<IRabbitMqService>();
            _pagamentoGateway = Substitute.For<IPagamentoGateway>();

            _pedidoRepository.BuscarTodos().Returns(_pedidoFixture.GerarPedidosValidos());
            var pedidoController = new PedidoController(
                _pedidoRepository,
                new PedidoPresenter(),
                _pedidoFixture.StatusPedidoValidacaoService!,
                _rabbitMqService,
                _pagamentoGateway);

            _pedidosResponseDto = await pedidoController.BuscarTodos();
        }

        #endregion

        #region Then

        private void Then_PedidoResponseCpfDeveTerMesmoCpfDoPedido()
        {
            Assert.Equal(_cpf.Numero, _pedidoResponseDto.ClienteCpf);
        }

        private void Then_PedidoResponseDtoNaoDeveSerNulo()
        {
            Assert.NotNull(_pedidoResponseDto);
        }

        private void Then_PedidosResponseDtoNaoDeveSerNulo()
        {
            Assert.NotNull(_pedidosResponseDto);
        }

        private void Then_StatusPedidoResponseDtoDeveSerIgualPedidoRecebido()
        {
            Assert.Equal(StatusPedido.PedidoRecebido, _pedidoResponseDto.StatusPedido);
        }

        private void Then_DevePublicarMensagemTrocaStatus()
        {
            _rabbitMqService.Received(1).Publicar(Arg.Any<IBaseMessage>());
        }

        private void Then_DeveLancarDomainException()
        {
            Assert.NotNull(_domainException);
        }

        private void Then_ExceptionDeveConterMensagem(string msg)
        {
            Assert.Equal(msg, _domainException.Message);
        }

        private void Then_DeveRetornarListaDePedidosNaoVazia()
        {
            Assert.NotNull(_pedidos);
            Assert.True(_pedidos.Any());
        }

        #endregion
    }
}
