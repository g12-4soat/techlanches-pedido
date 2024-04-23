using TechLanches.Adapter.API.Endpoints;

namespace TechLanches.Adapter.API.Configuration
{
    public static class MapEndpointsConfig
    {
        public static void UseMapEndpointsConfiguration(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPedidoEndpoints();
            endpoints.MapProdutoEndpoints();
            endpoints.MapCheckoutEndpoints();
        }
    }
}
