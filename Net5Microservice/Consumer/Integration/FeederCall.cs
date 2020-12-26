using System.Net;

namespace Consumer.Integration
{
    public class FeederCall
    {
        public static void SendToFeeder(string queueName, string message)
        {
            var uri = Startup.StaticConfig.GetSection("Parameters")["Feeder_Url"];
            var url = "http://" + uri + "/Feeder/" + queueName + "?message=" + message;
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.GetResponse();
        }
    }
}