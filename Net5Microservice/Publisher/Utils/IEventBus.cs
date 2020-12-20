using Publisher.Entity;

namespace Publisher.Utils
{
    public interface IEventBus
    {
        void SendToQueue(QueueMessage message);
    }
}
