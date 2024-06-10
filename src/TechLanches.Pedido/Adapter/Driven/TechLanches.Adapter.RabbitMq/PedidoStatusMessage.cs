using System.Text.Json;
using TechLanches.Domain.Enums;

namespace TechLanches.Adapter.RabbitMq
{
    public class PedidoStatusMessage : IBaseMessage
    {
        public PedidoStatusMessage(int pedidoId, StatusPedido statusPedido)
        {
            PedidoId = pedidoId;
            StatusPedido = statusPedido;
        }

        public int PedidoId { get; private set; }
        public StatusPedido StatusPedido { get; private set; }

        public string Type => nameof(PedidoStatusMessage);

        public string GetMessage()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
