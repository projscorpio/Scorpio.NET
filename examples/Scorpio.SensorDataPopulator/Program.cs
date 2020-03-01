using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.SensorDataPopulator
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Work().Wait();
            Console.Read();
        }

        private async Task Work()
        {
            const double minValue = 21.1111d;
            const double maxValue = 21.2321d;

            var client = new HttpClient();
            var random = new Random();
            var time = DateTime.UtcNow;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            for(var i = 0; i < 1000; i++)
            {
                var value = ChangeRange(random.NextDouble(), 0.0d, 1.0d, minValue, maxValue);
                time = time.AddSeconds(15.0d);
                Console.WriteLine($"i: {i}    val: {value}");
                await DoRequest(client, time, "voltage", value);
            }
        }
        
        private async Task DoRequest(HttpClient client, DateTime time, string sensorKey, double value)
        {
            const string url = "http://localhost:8080/api/sensorData";
            var body = JsonConvert.SerializeObject(new
            {
                sensorKey = sensorKey,
                value = value,
                timeStamp = time
            });

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            try
            {
                Console.WriteLine($"Sending: " + body);
                var resp = await client.SendAsync(request);
                resp.EnsureSuccessStatusCode();
                Console.WriteLine(resp.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static double ChangeRange(double x, double inMin, double inMax, double outMin, double outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
    }
}
