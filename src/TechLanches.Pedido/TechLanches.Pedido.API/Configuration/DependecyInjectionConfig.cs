using TechLanches.Adapter.RabbitMq.Messaging;
using TechLanches.Adapter.SqlServer.Repositories;
using TechLanches.Application.Controllers;
using TechLanches.Application.Controllers.Interfaces;
using TechLanches.Application.Gateways;
using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Application.Ports.Repositories;
using TechLanches.Application.Presenters;
using TechLanches.Application.Presenters.Interfaces;
using TechLanches.Domain.Services;
using TechLanches.Domain.Validations;
using TechLanchesPedido.API.Middlewares;

namespace TechLanches.Adapter.API.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IStatusPedidoValidacao, StatusPedidoCriadoValidacao>();
            services.AddSingleton<IStatusPedidoValidacao, StatusPedidoCanceladoValidacao>();
            services.AddSingleton<IStatusPedidoValidacao, StatusPedidoCanceladoPorPagamentoValidacao>();
            services.AddSingleton<IStatusPedidoValidacao, StatusPedidoEmPreparacaoValidacao>();
            services.AddSingleton<IStatusPedidoValidacao, StatusPedidoDescartadoValidacao>();
            services.AddSingleton<IStatusPedidoValidacao, StatusPedidoFinalizadoValidacao>();
            services.AddSingleton<IStatusPedidoValidacao, StatusPedidoProntoValidacao>();
            services.AddSingleton<IStatusPedidoValidacao, StatusPedidoRecebidoValidacao>();
            services.AddSingleton<IStatusPedidoValidacao, StatusPedidoRetiradoValidacao>();

            services.AddSingleton<IPagamentoGateway, PagamentoGateway>();

            services.AddSingleton<IProdutoPresenter, ProdutoPresenter>();
            services.AddSingleton<IPedidoPresenter, PedidoPresenter>();

            services.AddSingleton<IProdutoController, ProdutoController>();
            services.AddSingleton<IPedidoController, PedidoController>();
            services.AddSingleton<ICheckoutController, CheckoutController>();            

            services.AddSingleton<IStatusPedidoValidacaoService, StatusPedidoValidacaoService>();

            services.AddSingleton<IPedidoRepository, PedidoRepository>();
            services.AddSingleton<IProdutoRepository, ProdutoRepository>();

            services.AddSingleton<IRabbitMqService, RabbitMqService>();

            services.AddSingleton<JwtTokenMiddleware>();
        }
    }
}