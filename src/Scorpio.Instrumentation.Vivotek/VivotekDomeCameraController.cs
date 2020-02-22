using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Scorpio.Instrumentation.Vivotek.DomeCamera;

namespace Scorpio.Instrumentation.Vivotek
{
    public class VivotekDomeCameraController : HttpClientBase
    {
        public string ApiUrl { get; set; }

        public VivotekDomeCameraController(ILogger<VivotekDomeCameraController> logger) : base(logger)
        {
        }

        public async Task Control(CameraCommand command)
        {
            var queryParam = CommandsDictionary.Commands[command];
            var endpoint = ApiUrl + queryParam;

            await GetRawWithBasicAuthAsync(endpoint);
        }

        public async Task SetSpeed(CameraSpeedCommand command, sbyte speed)
        {
            if (speed > 5 || speed < -5)
                throw new ArgumentException("Speed value should be between -5 and 5");

            var queryParam = CommandsDictionary.SpeedCommands[command];
            var endpoint = ApiUrl + queryParam + speed;

            await GetRawWithBasicAuthAsync(endpoint);
        }

        private async Task GetRawWithBasicAuthAsync(string endpoint)
        {
            try
            {
                SetBasicAuthHeaders();
                Logger.LogInformation($"Sending camera command, constructed url: {endpoint}");
                await GetRawAsync(endpoint);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message, nameof(VivotekDomeCameraController));
            }
        }
    }
}
