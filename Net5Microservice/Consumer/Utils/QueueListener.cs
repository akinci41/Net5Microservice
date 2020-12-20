using Consumer.Integration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;

namespace Consumer.Utils
{
    public class QueueListener
    {
        ConnectionFactory Factory { get; set; }
        IConnection Connection { get; set; }
        IModel Channel { get; set; }

        public void Register()
        {
            Channel.QueueDeclare(queue: "AddContact", durable: false, exclusive: false, autoDelete: false, arguments: null);
            Channel.QueueDeclare(queue: "RemoveContact", durable: false, exclusive: false, autoDelete: false, arguments: null);
            Channel.QueueDeclare(queue: "AddCommunication", durable: false, exclusive: false, autoDelete: false, arguments: null);
            Channel.QueueDeclare(queue: "RemoveCommunication", durable: false, exclusive: false, autoDelete: false, arguments: null);
            Channel.QueueDeclare(queue: "GetReport", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer1 = new EventingBasicConsumer(Channel);
            consumer1.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                FeederCall.SendToFeeder("AddContact", message);
            };
            var consumer2 = new EventingBasicConsumer(Channel);
            consumer2.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                FeederCall.SendToFeeder("RemoveContact", message);
            };
            var consumer3 = new EventingBasicConsumer(Channel);
            consumer3.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                FeederCall.SendToFeeder("AddCommunication", message);
            };
            var consumer4 = new EventingBasicConsumer(Channel);
            consumer4.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                FeederCall.SendToFeeder("RemoveCommunication", message);
            };
            var consumer5 = new EventingBasicConsumer(Channel);
            consumer5.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                FeederCall.SendToFeeder("GetReport", "");
            };

            Channel.BasicConsume(queue: "AddContact", autoAck: true, consumer: consumer1);
            Channel.BasicConsume(queue: "RemoveContact", autoAck: true, consumer: consumer2);
            Channel.BasicConsume(queue: "AddCommunication", autoAck: true, consumer: consumer3);
            Channel.BasicConsume(queue: "RemoveCommunication", autoAck: true, consumer: consumer4);
            Channel.BasicConsume(queue: "GetReport", autoAck: true, consumer: consumer5);
        }

        public void Deregister()
        {
            this.Connection.Close();
        }

        public QueueListener()
        {
            var uri = Environment.GetEnvironmentVariable("RabbitMQ_Url");

            this.Factory = new ConnectionFactory() { HostName = uri };
            this.Connection = Factory.CreateConnection();
            this.Channel = Connection.CreateModel();
        }
    }
}
