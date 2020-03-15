using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Scorpio.Instrumentation.Vivotek
{
    public class HttpClientBase : IDisposable
    {
        private NetworkCredential _credentials;
        public NetworkCredential Credentials
        {
            get
            {
                if (_credentials is null
                  || string.IsNullOrEmpty(_credentials.Password)
                  || string.IsNullOrEmpty(_credentials.UserName))
                    throw new ArgumentNullException("Please set credentials first");

                return _credentials;
            }
            set => _credentials = value;
        }

        protected readonly HttpClient Client;
        protected readonly ILogger<HttpClientBase> Logger;

        public HttpClientBase(ILogger<HttpClientBase> logger)
        {
            Logger = logger;

            Client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5.0d)
            };

            Logger.LogDebug("Http client has been created.");
        }

        public async Task<string> GetRawAsync(string url)
        {
            try
            {
                var response = await Client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                Logger.LogError($"Unexpected response from Vivotek: {response.ReasonPhrase}");
            }
            catch (HttpRequestException ex)
            {
                Logger.LogError(ex, "Could not communicate with given device");
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogError(ex, "Timeout occured");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
            
            return null;
        }

        protected void SetBasicAuthHeaders()
        {
            var creds = Base64Encode($"{Credentials.UserName}:{Credentials.Password}");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds);
        }

        protected static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
