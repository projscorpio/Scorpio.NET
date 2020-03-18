using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scorpio.Api.DataAccess;
using Scorpio.Api.Hubs;
using Scorpio.Api.Models;
using Scorpio.Api.Validation;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Scorpio.Api.EventHandlers
{
    public class GpsDataReceivedEventHandler : IIntegrationEventHandler<GpsDataReceivedEvent>
    {
        private readonly ISensorDataRepository _sensorDataRepository;
        private readonly IHubContext<MainHub> _hubContext;
        private readonly ILogger<GpsDataReceivedEventHandler> _logger;

        public GpsDataReceivedEventHandler(ISensorDataRepository sensorDataRepository, IHubContext<MainHub> hubContext,
            ILogger<GpsDataReceivedEventHandler> logger)
        {
            _sensorDataRepository = sensorDataRepository;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task Handle(GpsDataReceivedEvent @event)
        {
            _logger.LogDebug("Received GPS data from broker...");

            var sensorDataEntity = new SensorData
            {
                SensorKey = "gps",
                TimeStamp = DateTime.UtcNow,
                Value = JsonConvert.SerializeObject(new GpsData
                {
                    Latitude = @event.Latitude,
                    Longitude = @event.Longitude
                })
            };

            try
            {
                // Validate payload
                new GpsDataValidator().Validate(sensorDataEntity);

                // Save to DB
                await _sensorDataRepository.CreateAsync(sensorDataEntity);

                // Notify clients via SignalR
                await _hubContext.Clients.All.SendAsync(Constants.Topics.GpsPosition, sensorDataEntity.Value);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Received invalid GPS data!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown error at {nameof(GpsDataReceivedEventHandler)}");
            }
        }
    }
}
