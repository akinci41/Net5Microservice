using System;
using System.Net;

namespace Consumer.Integration
{
    public class FeederCall
    {
        private static readonly string uri = Environment.GetEnvironmentVariable("Feeder_Url");
        public static void SendToFeeder(string queueName, string message)
        {
            var url = "http://" + uri + "/Feeder/" + queueName + "?message=" + message;
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";   
            httpRequest.GetResponse();
        }
    }
}
