using System.Text.Json;

namespace TechLanches.Adapter.RabbitMq
{
    public class PedidoMessage : IBaseMessage
    {
        public int Id { get; private set; }
        public string Cpf { get; private set; }

        public Type Type => typeof(PedidoMessage);

        public PedidoMessage(int id, string cpf)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cpf);

            Id = id;
            Cpf = cpf;
        }

        public string GetMessage()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
