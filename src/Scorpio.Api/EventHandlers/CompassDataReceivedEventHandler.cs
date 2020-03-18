using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Scorpio.Api.DataAccess;
using Scorpio.Api.Hubs;
using Scorpio.Api.Models;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using System;
using System.Threading.Tasks;

namespace Scorpio.Api.EventHandlers
{
    public class CompassDataReceivedEventHandler : IIntegrationEventHandler<CompassDataReceivedEvent>
    {
        private readonly ISensorDataRepository _sensorDataRepository;
        private readonly IHubContext<MainHub> _hubContext;
        private readonly ILogger<GpsDataReceivedEventHandler> _logger;

        public CompassDataReceivedEventHandler(ISensorDataRepository sensorDataRepository, IHubContext<MainHub> hubContext,
            ILogger<GpsDataReceivedEventHandler> logger)
        {
            _sensorDataRepository = sensorDataRepository;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task Handle(CompassDataReceivedEvent @event)
        {
            _logger.LogDebug("Received compass data from broker...");

            var sensorDataEntity = new SensorData
            {
                SensorKey = "compass",
                TimeStamp = DateTime.UtcNow,
                Value = @event.Angle.ToString()
            };

            try
            {
                // Save to DB
                await _sensorDataRepository.CreateAsync(sensorDataEntity);

                // Notify clients via SignalR
                await _hubContext.Clients.All.SendAsync(Constants.Topics.Compass, sensorDataEntity.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown error at {nameof(CompassDataReceivedEventHandler)}");
            }
        }
    }
}
