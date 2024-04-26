namespace TechLanches.Adapter.RabbitMq
{
    public interface IBaseMessage
    {
        string Type { get; }
        string GetMessage();
    }
}
