using Autofac;
using Microsoft.Extensions.Logging;
using Scorpio.Instrumentation.Ubiquiti;
using Scorpio.Messaging.Messages;
using System;

namespace Scorpio.Api.HostedServices
{
    public class UbiquitiPollerHostedService : HostedServiceBase
    {
        protected override TimeSpan TaskTimeout => TimeSpan.FromSeconds(4.5d);
        protected override TimeSpan TaskPeriod => TimeSpan.FromSeconds(5.0d);

        private readonly UbiquitiStatsProvider _ubiProvider;

        public UbiquitiPollerHostedService(ILifetimeScope autofac) : base(autofac)
        {
            _ubiProvider = Autofac.Resolve<UbiquitiStatsProvider>();
        }

        protected override async void DoWork(object state)
        {
            try
            {
                var stats = await _ubiProvider.GetStatsAsync(CancellationToken);

                if (stats != null && stats.Count > 0)
                {
                    EventBus?.Publish(new UbiquitiDataReceivedEvent(stats));
                }
            }
            catch (OperationCanceledException ex)
            {
                const string msg = "Could not connect to SNMP host - timeout occured";
                Logger.LogError(ex, msg);
            }
            catch (Exception ex)
            {
                const string msg = "Could not connect to SNMP host - make sure it is in the same network";
                Logger.LogError(ex, msg);
            }
        }
    }
}
