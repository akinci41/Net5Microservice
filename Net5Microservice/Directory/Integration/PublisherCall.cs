using Directory.Entity;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Directory.Integration
{
    public class PublisherCall
    {
        private static readonly string uri = Environment.GetEnvironmentVariable("Publisher_Url");
        public static void SendToQueue(string queueName, object msg)
        {
            var url = "http://" + uri + "/Publisher";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var obj = JsonSerializer.Serialize(msg);
            var message = new QueueMessage { QueueName = queueName, Object = obj };
            var data = JsonSerializer.Serialize(message);
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }
            httpRequest.GetResponse();
        }
    }
}
