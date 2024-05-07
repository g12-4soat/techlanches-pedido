namespace TechLanches.Adapter.RabbitMq.Messaging
{
    public interface IRabbitMqService
    {
        void Publicar(IBaseMessage baseMessage);
    }
}
