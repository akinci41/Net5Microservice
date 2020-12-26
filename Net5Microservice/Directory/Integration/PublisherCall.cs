using System.Net;
using System.Text.Json;

namespace Directory.Integration
{
    public class PublisherCall
    {
        public static void SendToQueue(string queueName, object obj)
        {
            var uri = Startup.StaticConfig.GetSection("Parameters")["Publisher_Url"];
            var message = JsonSerializer.Serialize(obj);
            var url = "http://" + uri + "/Publisher?queueName=" + queueName + "&message=" + message;
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.GetResponse();
        }
    }
}