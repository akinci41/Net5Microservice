using RabbitMQ.Client;
using System;
using System.Text;
using Publisher.Entity;

namespace Publisher.Utils
{
    public class EventBus : IEventBus
    {
        public void SendToQueue(QueueMessage message)
        {
            var uri = Environment.GetEnvironmentVariable("RabbitMQ_Url");

            var factory = new ConnectionFactory() { HostName = uri };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: message.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var body = Encoding.UTF8.GetBytes(message.Object);
                    channel.BasicPublish(exchange: "", routingKey: message.QueueName, basicProperties: null, body: body);
                }
            }
        }
    }
}
