using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scorpio.CanOpenEds.FileRepository;
using System;

namespace Scorpio.CanOpenEds.Extensions.DependencyInjection.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCanOpenFileEds(this IServiceCollection serviceCollection, FileRepositoryConfiguration config)
        {
            if (serviceCollection is null)
                throw new ArgumentException(nameof(IServiceCollection));

            serviceCollection.AddSingleton<ICanOpenObjectRepository, CanObjectFileRepository>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<CanObjectFileRepository>>();
                var memoryCache = provider.GetRequiredService<IMemoryCache>();

                return new CanObjectFileRepository(logger, memoryCache, config);
            });

            return serviceCollection;
        }
    }
}
