using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scorpio.Api.DataAccess.Seeding;
using System;

namespace Scorpio.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .Build();

            var seeder = (IDbSeeder) host.Services.GetService(typeof(IDbSeeder));
            var logger = (ILogger<Program>)host.Services.GetService(typeof(ILogger<Program>));

            try
            {
                seeder.Seed().Wait();
                host.Run();
            }
            catch (AggregateException ex)
            {
                logger.LogError(ex, "Fatal error while bootstrapping app.");

                if (ex.InnerException is TimeoutException)
                {
                    logger.LogError("Timeout while connecting to database. Ensure database exists and ConnectionString is correct. Stopping.");
                }

                // Fatal - cannot proceed
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
