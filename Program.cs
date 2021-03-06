using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttPowerState
{
    class Program
    {
        public static bool CheckForInternetConnection(int timeoutMs = 10000, string url = null)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using var response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            string command;
            if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "wake":
                    case "sleep":
                        command = args[0];
                        break;
                    default:
                        throw new ArgumentException("Invalid argument. Must be 'wake' or 'sleep'.");
                }
            }
            else
            {
                throw new ArgumentException("Invalid argument. Must be 'wake' or 'sleep'.");
            }

            var tries = 0;
            while (tries < 3 && !CheckForInternetConnection(10000, "http://www.gstatic.com/generate_204"))
            {
                tries++;
            }

            var host = Environment.GetEnvironmentVariable("MQTT_HOST");
            var port = int.Parse(Environment.GetEnvironmentVariable("MQTT_PORT"));
            var user = Environment.GetEnvironmentVariable("MQTT_USERNAME");
            var password = Environment.GetEnvironmentVariable("MQTT_PWD");
            // create client instance 
            var client = new MqttClient(host, port, false, null, null, MqttSslProtocols.TLSv1_2, null);
            client.Connect("MqttPowerState", user, password);
            client.Publish("windows", Encoding.UTF8.GetBytes(command), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
            Thread.Sleep(250);
            client.Disconnect();
        }


    }
}
