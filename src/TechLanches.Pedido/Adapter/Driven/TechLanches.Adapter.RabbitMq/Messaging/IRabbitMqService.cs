namespace TechLanches.Adapter.RabbitMq.Messaging
{
    public interface IRabbitMqService
    {
        void Publicar(IBaseMessage baseMessage);
        Task Consumir(Func<string, Task> function);
    }
}
