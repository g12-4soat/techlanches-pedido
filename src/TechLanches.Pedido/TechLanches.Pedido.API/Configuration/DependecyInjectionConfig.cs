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

namespace TechLanches.Adapter.API.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IStatusPedidoValidacao, StatusPedidoCriadoValidacao>();
            services.AddScoped<IStatusPedidoValidacao, StatusPedidoCanceladoValidacao>();
            services.AddScoped<IStatusPedidoValidacao, StatusPedidoCanceladoPorPagamentoValidacao>();
            services.AddScoped<IStatusPedidoValidacao, StatusPedidoEmPreparacaoValidacao>();
            services.AddScoped<IStatusPedidoValidacao, StatusPedidoDescartadoValidacao>();
            services.AddScoped<IStatusPedidoValidacao, StatusPedidoFinalizadoValidacao>();
            services.AddScoped<IStatusPedidoValidacao, StatusPedidoProntoValidacao>();
            services.AddScoped<IStatusPedidoValidacao, StatusPedidoRecebidoValidacao>();
            services.AddScoped<IStatusPedidoValidacao, StatusPedidoRetiradoValidacao>();

            services.AddScoped<IPagamentoGateway, PagamentoGateway>();

            services.AddSingleton<IProdutoPresenter, ProdutoPresenter>();
            services.AddSingleton<IPedidoPresenter, PedidoPresenter>();

            services.AddScoped<IProdutoController, ProdutoController>();
            services.AddScoped<IPedidoController, PedidoController>();
            services.AddScoped<ICheckoutController, CheckoutController>();            

            services.AddScoped<IStatusPedidoValidacaoService, StatusPedidoValidacaoService>();

            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            services.AddSingleton<IRabbitMqService, RabbitMqService>();            
        }
    }
}