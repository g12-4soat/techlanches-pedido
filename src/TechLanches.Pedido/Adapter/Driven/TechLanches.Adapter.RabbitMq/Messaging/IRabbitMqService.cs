namespace TechLanches.Adapter.RabbitMq.Messaging
{
    public interface IRabbitMqService
    {
        void Publicar(IBaseMessage baseMessage, string queueName);
        Task Consumir(Func<PedidoStatusMessage, Task> function);
    }
}
