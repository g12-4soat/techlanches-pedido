using Microsoft.Extensions.Hosting;
using TechLanches.Adapter.RabbitMq.Messaging;
using TechLanches.Application.Controllers.Interfaces;

namespace TechLanches.Pedido.Consumer
{
    internal class PedidoConsumerHostedService : BackgroundService
    {
        private readonly IPedidoController _pedidoController;
        private readonly IRabbitMqService _rabbitMqService;

        public PedidoConsumerHostedService(IPedidoController pedidoController, IRabbitMqService rabbitMqService)
        {
            _pedidoController = pedidoController;
            _rabbitMqService = rabbitMqService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _rabbitMqService.Consumir(_pedidoController.ProcessarMensagem);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
