namespace Publisher.Utils
{
    public interface IEventBus
    {
        void SendToQueue(string queueName, string message);
    }
}
