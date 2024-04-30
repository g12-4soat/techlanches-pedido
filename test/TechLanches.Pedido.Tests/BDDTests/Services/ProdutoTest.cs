using TechLanches.Application.DTOs;

namespace TechLanches.Pedido.Tests.BDDTests.Services
{
    [Trait("Services", "Produto")]
    public class ProdutoTest
    {
        private Produto _produto;
        private ProdutoResponseDTO _novoProdutoDto;

        [Fact(DisplayName = "Deve cadastrar produto com sucesso")]
        public async Task CadastrarProduto_DeveRetornarSucesso()
        {
            Given_ProdutoComDadosValidos();
            await When_CadastrarProduto();
            await When_CadastrarProduto();
            Then_ProdutoCriadoNaoDeveSerNulo();
            Then_TodosAsPropriedadesDevemSerIguais();
        }

        //[Fact(DisplayName = "Deve atualizar produto com sucesso")]
        //public async Task AtualizarProduto_DeveRetornarSucesso()
        //{
        //    // Arrange
        //    var produto = new Produto("Novo Nome", "Nova Descrição", 20, 2);
        //    var produtoRepository = Substitute.For<IProdutoRepository>();
        //    var unitOfWork = Substitute.For<IUnitOfWork>();
        //    produtoRepository.UnitOfWork.Returns(unitOfWork);
        //    produtoRepository.BuscarPorId(Arg.Any<int>()).Returns(produto);
        //    var produtoController = new ProdutoController(produtoRepository, new ProdutoPresenter());

        //    // Act
        //    await produtoController.Atualizar(0, "Novo Nome", "Nova Descrição", 20, 2);

        //    // Assert
        //    await unitOfWork.Received(1).CommitAsync();
        //}

        //[Fact(DisplayName = "Deve buscar produtos por categoria com sucesso")]
        //public async Task BuscarPorCategoria_DeveRetornarProdutosComCategoriaSolicitada()
        //{
        //    // Arrange
        //    var produtoRepository = Substitute.For<IProdutoRepository>();
        //    var unitOfWork = Substitute.For<IUnitOfWork>();
        //    produtoRepository.UnitOfWork.Returns(unitOfWork);
        //    produtoRepository.BuscarPorCategoria(new CategoriaProduto(1, "teste")).Returns(new List<Produto> { new ("Nome", "Descrição do produto", 20.0m, 2) });

        //    var produtoController = new ProdutoController(produtoRepository, new ProdutoPresenter());

        //    // Act
        //    var listaDeProdutos = await produtoController.BuscarPorCategoria(1);

        //    // Assert
        //    await produtoRepository.Received(1).BuscarPorCategoria(Arg.Any<CategoriaProduto>());
        //    Assert.NotNull(listaDeProdutos);
        //}

        //[Fact(DisplayName = "Deve buscar produto por id com sucesso")]
        //public async Task BuscarPorId_DeveRetornarProdutoSolicitado()
        //{
        //    // Arrange
        //    var produto = new Produto("Nome", "Descrição do produto", 20, 2);
        //    var produtoRepository = Substitute.For<IProdutoRepository>();
        //    var unitOfWork = Substitute.For<IUnitOfWork>();
        //    produtoRepository.UnitOfWork.Returns(unitOfWork);
        //    produtoRepository.BuscarPorId(1).Returns(produto);

        //    var produtoController = new ProdutoController(produtoRepository, new ProdutoPresenter());

        //    // Act
        //    var produtoAtualizado = await produtoController.BuscarPorId(1);

        //    // Assert
        //    await produtoRepository.Received(1).BuscarPorId(1);
        //    Assert.NotNull(produtoAtualizado);
        //    Assert.IsType<ProdutoResponseDTO>(produtoAtualizado);
        //    Assert.Equal(produto.Nome, produtoAtualizado.Nome);
        //}

        //[Fact(DisplayName = "Deve buscar todos produtos com sucesso")]
        //public async Task BuscarTodos_DeveRetornarTodosProdutos()
        //{
        //    // Arrange
        //    var produtoRepository = Substitute.For<IProdutoRepository>();
        //    var unitOfWork = Substitute.For<IUnitOfWork>();
        //    produtoRepository.UnitOfWork.Returns(unitOfWork);
        //    produtoRepository.BuscarTodos().Returns(new List<Produto> { new ("Nome", "Descrição do produto", 20, 2) });

        //    var produtoController = new ProdutoController(produtoRepository, new ProdutoPresenter());

        //    // Act
        //    var listaDeProdutos = await produtoController.BuscarTodos();

        //    // Assert
        //    await produtoRepository.Received(1).BuscarTodos();
        //    Assert.NotNull(listaDeProdutos);
        //    Assert.IsType<List<ProdutoResponseDTO>>(listaDeProdutos);
        //}

        //[Fact(DisplayName = "Deve deletar o produto com sucesso")]
        //public async Task Deletar_ProdutoEncontrado_DeveDeletarProdutoComSucesso()
        //{
        //    // Arrange
        //    var produto = new Produto("Nome", "Descrição do produto", 20, 2);
        //    var produtoRepository = Substitute.For<IProdutoRepository>();
        //    var unitOfWork = Substitute.For<IUnitOfWork>();
        //    produtoRepository.UnitOfWork.Returns(unitOfWork);
        //    produtoRepository.BuscarPorId(1).Returns(produto);

        //    var produtoController = new ProdutoController(produtoRepository, new ProdutoPresenter());

        //    // Act
        //    await produtoController.Deletar(1);

        //    // Assert
        //    await unitOfWork.Received(1).CommitAsync();
        //    Assert.NotNull(produto);
        //    Assert.True(produto.Deletado);
        //}

        private void Given_ProdutoComDadosValidos()
        {
            string nome = "Produto Teste";
            string descricao = "Descrição do produto teste";
            decimal preco = 10.0M;
            int categoriaId = 1;

            _produto = new Produto(nome, descricao, preco, categoriaId);
        }

        private async Task When_CadastrarProduto()
        {
            var produtoRepository = Substitute.For<IProdutoRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            produtoRepository.UnitOfWork.Returns(unitOfWork);

            produtoRepository.Cadastrar(_produto).Returns(_produto);

            var produtoController = new ProdutoController(produtoRepository, new ProdutoPresenter());

            _novoProdutoDto = await produtoController.Cadastrar(_produto.Nome, _produto.Descricao, _produto.Preco, _produto.Categoria.Id);
        }

        private void Then_ProdutoCriadoNaoDeveSerNulo()
        {
            Assert.NotNull(_novoProdutoDto);
        }

        private void Then_TodosAsPropriedadesDevemSerIguais()
        {
            Assert.Equal(_produto.Nome, _novoProdutoDto.Nome);
            Assert.Equal(_produto.Descricao, _novoProdutoDto.Descricao);
            Assert.Equal(_produto.Preco, _novoProdutoDto.Preco);
            Assert.Equal(CategoriaProduto.From(_produto.Categoria.Id).Nome, _novoProdutoDto.Categoria);
        }
    }
}
