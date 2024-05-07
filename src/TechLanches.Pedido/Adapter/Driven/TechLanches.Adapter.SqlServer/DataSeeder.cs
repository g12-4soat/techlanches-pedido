using TechLanches.Domain.Aggregates;
using TechLanches.Domain.Constantes;
using TechLanches.Domain.ValueObjects;

namespace TechLanches.Adapter.SqlServer
{
    public static class DataSeeder
    {
        public static void Seed(TechLanchesDbContext context)
        {
            ProdutosSeed(context);
            PedidosSeed(context);
        }

        private static void ProdutosSeed(TechLanchesDbContext context)
        {
            if (!context.Produtos.Any())
            {
                var produtos = new List<Produto>
                {
                    new ("X-Burguer", "Lanche com pão carne e queijo", 30, 1),
                    new ("Batata Frita", "Fritas comum", 8, 2),
                    new ("Suco de Laranja", "Suco natural de laranjas", 10, 3),
                    new ("Sorvete", "Casquinha sabor chocolate ou baunilha", 3, 4),
                };

                context.AddRange(produtos);
                context.SaveChanges();
            }
        }

        private static void PedidosSeed(TechLanchesDbContext context)
        {
            if (!context.Pedidos.Any())
            {
                var produto1 = context.Produtos.Find(1);
                var produto2 = context.Produtos.Find(2);
                var pedidos = new List<Pedido>
                {
                    new (new Cpf(Constants.CPF_USER_DEFAULT), new List<ItemPedido> { new (produto1!.Id, 1, produto1.Preco), new (produto2!.Id, 2, produto2.Preco) })
                };

                context.AddRange(pedidos);
                context.SaveChanges();
            }
        }
    }
}