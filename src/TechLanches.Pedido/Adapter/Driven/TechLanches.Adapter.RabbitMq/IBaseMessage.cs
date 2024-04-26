namespace TechLanches.Adapter.RabbitMq
{
    public interface IBaseMessage
    {
        Type Type { get; }
        string GetMessage();
    }
}
