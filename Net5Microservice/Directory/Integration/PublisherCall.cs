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
        public static void SendToQueue(string queueName, object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            var url = "http://" + uri + "/Publisher?queueName=" + queueName + "&message=" + message;
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.GetResponse();
        }
    }
}
