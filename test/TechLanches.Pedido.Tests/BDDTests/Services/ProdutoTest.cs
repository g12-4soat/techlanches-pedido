namespace TechLanchesPedido.Tests.BDDTests.Services
{
    [Trait("Services", "Produto")]
    public class ProdutoTest
    {
        private Produto _produto;
        private List<Produto> _produtos;
        private List<ProdutoResponseDTO> _produtosResponseDto;
        private CategoriaProduto _categoriaProduto;
        private ProdutoResponseDTO _produtoResponseDto;
        private ProdutoController _produtoController;
        private IProdutoRepository _produtoRepository;
        private IUnitOfWork _unitOfWork;

        [Fact(DisplayName = "Deve cadastrar produto com sucesso")]
        public async Task CadastrarProduto_DeveRetornarSucesso()
        {
            Given_ProdutoComDadosValidos();
            await When_CadastrarProduto();
            await When_CadastrarProduto();
            Then_ProdutoDtoCriadoNaoDeveSerNulo();
            Then_TodosAsPropriedadesDevemSerIguais();
        }

        [Fact(DisplayName = "Deve atualizar produto com sucesso")]
        public async Task AtualizarProduto_DeveRetornarSucesso()
        {
            Given_ProdutoComDadosValidos();
            await When_AtualizarProduto();
            await Then_DeveRealizarCommit();
        }

        [Fact(DisplayName = "Deve buscar produtos por categoria com sucesso")]
        public async Task BuscarProdutosPorCategoria_DeveRetornarProdutosComCategoriaSolicitada()
        {
            Given_CategoriaProdutoValida();
            await When_BuscarProdutosPorCategoria();
            Then_ListaProdutoDtoNaoDeveSerNula();
        }

        [Fact(DisplayName = "Deve buscar produto por id com sucesso")]
        public async Task BuscarPorId_DeveRetornarProdutoSolicitado()
        {
            Given_ProdutoComDadosValidos();
            await When_BuscarProdutoPorId();
            Then_ProdutoNaoDeveSerNulo();
            await Then_DeveTerMesmoNomeCadastrado();
        }

        [Fact(DisplayName = "Deve buscar todos produtos com sucesso")]
        public async Task BuscarTodos_DeveRetornarTodosProdutos()
        {
            Given_ListaDeProdutosValida();
            await When_BuscarTodosProdutos();
            Then_ListaProdutoDtoNaoDeveSerNula();
        }

        [Fact(DisplayName = "Deve deletar o produto com sucesso")]
        public async Task Deletar_ProdutoEncontrado_DeveDeletarProdutoComSucesso()
        {
            Given_ProdutoComDadosValidos();
            await When_DeletarProduto();
            Then_ProdutoNaoDeveSerNulo();
            Then_FlagProdutoDeletadoDeveSerTrue();
        }

        #region Given

        private void Given_CategoriaProdutoValida()
        {
            _categoriaProduto = new CategoriaProduto(1, "teste");
        }

        private void Given_ProdutoComDadosValidos()
        {
            string nome = "Produto Teste";
            string descricao = "Descrição do produto teste";
            decimal preco = 10.0M;
            int categoriaId = 1;

            _produto = new Produto(nome, descricao, preco, categoriaId);
        }

        private void Given_ListaDeProdutosValida()
        {
            _produtos = new List<Produto> { new("Nome", "Descrição do produto", 20, 2) };
        }

        #endregion

        #region When

        private async Task When_BuscarProdutoPorId()
        {
            _produtoRepository = Substitute.For<IProdutoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _produtoRepository.UnitOfWork.Returns(_unitOfWork);
            _produtoRepository.BuscarPorId(1).Returns(_produto);

            var produtoController = new ProdutoController(_produtoRepository, new ProdutoPresenter());

            // Act
            _produtoResponseDto = await produtoController.BuscarPorId(1);
        }

        private async Task When_BuscarProdutosPorCategoria()
        {
            _produtoRepository = Substitute.For<IProdutoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _produtoRepository.UnitOfWork.Returns(_unitOfWork);
            _produtoRepository.BuscarPorCategoria(_categoriaProduto).Returns(new List<Produto> { new("Nome", "Descrição do produto", 20.0m, 2) });

            var produtoController = new ProdutoController(_produtoRepository, new ProdutoPresenter());

            _produtosResponseDto = await produtoController.BuscarPorCategoria(1);

            await _produtoRepository.Received(1).BuscarPorCategoria(Arg.Any<CategoriaProduto>());
        }

        private async Task When_DeletarProduto()
        {
            _produtoRepository = Substitute.For<IProdutoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _produtoRepository.UnitOfWork.Returns(_unitOfWork);
            _produtoRepository.BuscarPorId(1).Returns(_produto);

            _produtoController = new ProdutoController(_produtoRepository, new ProdutoPresenter());

            await _produtoController.Deletar(1);
        }

        private async Task When_CadastrarProduto()
        {
            _produtoRepository = Substitute.For<IProdutoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _produtoRepository.UnitOfWork.Returns(_unitOfWork);

            _produtoRepository.Cadastrar(_produto).Returns(_produto);

            var produtoController = new ProdutoController(_produtoRepository, new ProdutoPresenter());

            _produtoResponseDto = await produtoController.Cadastrar(_produto.Nome, _produto.Descricao, _produto.Preco, _produto.Categoria.Id);
        }

        private async Task When_AtualizarProduto()
        {
            _produtoRepository = Substitute.For<IProdutoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _produtoRepository.UnitOfWork.Returns(_unitOfWork);
            _produtoRepository.BuscarPorId(Arg.Any<int>()).Returns(_produto);
            var produtoController = new ProdutoController(_produtoRepository, new ProdutoPresenter());

            // Act
            await produtoController.Atualizar(0, "Novo Nome", "Nova Descrição", 20, 2);
        }

        private async Task When_BuscarTodosProdutos()
        {
            _produtoRepository = Substitute.For<IProdutoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _produtoRepository.UnitOfWork.Returns(_unitOfWork);
            _produtoRepository.BuscarTodos().Returns(_produtos);

            var produtoController = new ProdutoController(_produtoRepository, new ProdutoPresenter());

            _produtosResponseDto = await produtoController.BuscarTodos();

            await _produtoRepository.Received(1).BuscarTodos();
        }

        #endregion

        #region Then

        private async Task Then_DeveTerMesmoNomeCadastrado()
        {
            await _produtoRepository.Received(1).BuscarPorId(1);
            Assert.IsType<ProdutoResponseDTO>(_produtoResponseDto);
            Assert.Equal(_produto.Nome, _produtoResponseDto.Nome);
        }

        private void Then_ListaProdutoDtoNaoDeveSerNula()
        {
            Assert.NotNull(_produtosResponseDto);
        }

        private void Then_ProdutoNaoDeveSerNulo()
        {
            Assert.NotNull(_produto);
        }

        private void Then_ProdutoDtoCriadoNaoDeveSerNulo()
        {
            Assert.NotNull(_produtoResponseDto);
        }

        private void Then_TodosAsPropriedadesDevemSerIguais()
        {
            Assert.Equal(_produto.Nome, _produtoResponseDto.Nome);
            Assert.Equal(_produto.Descricao, _produtoResponseDto.Descricao);
            Assert.Equal(_produto.Preco, _produtoResponseDto.Preco);
            Assert.Equal(CategoriaProduto.From(_produto.Categoria.Id).Nome, _produtoResponseDto.Categoria);
        }

        private async Task Then_DeveRealizarCommit()
        {
            await _unitOfWork.Received(1).CommitAsync();
        }

        private void Then_FlagProdutoDeletadoDeveSerTrue()
        {
            Assert.True(_produto.Deletado);
        }

        #endregion
    }
}
