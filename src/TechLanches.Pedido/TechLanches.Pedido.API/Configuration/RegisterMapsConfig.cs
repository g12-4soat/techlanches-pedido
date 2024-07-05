using Mapster;
using System.Reflection;
using TechLanches.Application.DTOs;
using TechLanches.Domain.Aggregates;
using TechLanches.Domain.ValueObjects;

namespace TechLanches.Adapter.API.Configuration
{
    public static class RegisterMapsConfig
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public static void RegisterMaps(this IServiceCollection services)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            TypeAdapterConfig<Produto, ProdutoResponseDTO>.NewConfig()
                .Map(dest => dest.Categoria, src => CategoriaProduto.From(src.Categoria.Id));

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            TypeAdapterConfig<TechLanches.Domain.Aggregates.Pedido, PedidoResponseDTO>.NewConfig()
                .Map(dest => dest.NomeStatusPedido, src => src.StatusPedido.ToString());
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8604 // Possible null reference argument.

            TypeAdapterConfig<ItemPedido, ItemPedidoResponseDTO>.NewConfig()
                .Map(dest => dest.Produto.Nome, src => src.Produto.Nome);

            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }
    }
}