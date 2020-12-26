using RabbitMQ.Client;
using System.Text;

namespace Publisher.Utils
{
    public class EventBus : IEventBus
    {
        public void SendToQueue(string queueName, string message)
        {
            var uri = Startup.StaticConfig.GetSection("Parameters")["RabbitMQ_Url"];

            var factory = new ConnectionFactory() { HostName = uri };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                }
            }
        }
    }
}
