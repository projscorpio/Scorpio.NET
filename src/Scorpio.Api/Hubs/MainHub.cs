using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scorpio.Api.Hubs
{
    public class MainHub : Hub
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<MainHub> _logger;

        #region Constructor
        public MainHub(IEventBus eventBus, ILogger<MainHub> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }
        #endregion

        #region Basic configuration
        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"New SignalR connection: {Context?.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogWarning($"SignalR user disconnected: {exception?.Message}");
            return base.OnDisconnectedAsync(exception);
        }
        #endregion

        #region Following methods are callable from the UI via SignalR

        [HubMethodName("RoverControlCommand")]
        public void RoverControlCommand(Dictionary<string, object> data)
        {
            const string accKey = "acc";
            const string dirKey = "dir";

            if (!data.ContainsKey(accKey) || !data.ContainsKey(dirKey)) return;

            if (float.TryParse(data[accKey].ToString(), out var acc) &&
                float.TryParse(data[dirKey].ToString(), out var dir))
            {
                var command = new RoverControlCommand(dir, acc); // {KeyOverride = "drive"};
                _logger.LogInformation($"Received SignalR data: {JsonConvert.SerializeObject(command)}");
                _eventBus.Publish(command);
            }

        }

        [HubMethodName("ArmRover")]
        public void ArmRover(Dictionary<string, object> data)
        {
            _logger.LogWarning($"Received SignalR call to ARM rover");
            _eventBus.Publish(new ArmRoverCommand());
        }

        [HubMethodName("DisarmRover")]
        public void DisarmRover(Dictionary<string, object> data)
        {
            _logger.LogWarning($"Received SignalR call to DISARM rover");
            _eventBus.Publish(new DisarmRoverCommand());
        }

        #endregion
    }
}
