using Matty.Framework;
using Matty.Framework.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Scorpio.Api.DataAccess;
using Scorpio.Api.DataAccess.Seeding;
using Scorpio.Api.EventHandlers;
using Scorpio.Api.HostedServices;
using Scorpio.Api.Hubs;
using Scorpio.CanOpenEds;
using Scorpio.CanOpenEds.Extensions.DependencyInjection.Microsoft;
using Scorpio.Gamepad.Processors;
using Scorpio.Gamepad.Processors.Mixing;
using Scorpio.Instrumentation.Ubiquiti;
using Scorpio.Messaging.Sockets;
using Scorpio.ProcessRunner;
using Scorpio.Reporting;
using Scorpio.Reporting.Pdf;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Scorpio.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the ConfigureContainer method, below
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Use NewtonSoft JSON as default serializer
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new ServiceResult<object>(context.ModelState
                        .Where(x => !string.IsNullOrEmpty(x.Value.Errors.FirstOrDefault()?.ErrorMessage))
                        .Select(x => new Alert(x.Value.Errors.FirstOrDefault()?.ErrorMessage, MessageType.Error))
                        .ToList());

                    return new BadRequestObjectResult(result);
                };
            });

            // SignalR - real time messaging with front end
            services.AddSignalR(settings =>
                {
                    settings.EnableDetailedErrors = true;
                    settings.KeepAliveInterval = TimeSpan.FromSeconds(5.0);
                    settings.ClientTimeoutInterval = TimeSpan.FromSeconds(20.0);
                })
                .AddMessagePackProtocol();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ScorpioAPI", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddLogging(opt => { opt.AddConsole(c => { c.TimestampFormat = "[HH:mm:ss:fff] "; }); });

            // This allows access http context and user in constructor
            services.AddHttpContextAccessor()
                .Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"))
                .Configure<MongoDbConfiguration>(Configuration.GetSection("MongoDb"))
                .Configure<UbiquitiPollerConfiguration>(Configuration.GetSection("Ubiquiti"))
                .Configure<SocketConfiguration>(Configuration.GetSection("socketClient"))
                .AddSocketClientConnection()
                .AddSocketClientEventBus()
                .AddTransient<IGenericProcessRunner, GenericProcessRunner>()
                .AddTransient<SaveSensorDataEventHandler>()
                .AddTransient<SaveManySensorDataEventHandler>()
                .AddTransient<UbiquitiDataReceivedEventHandler>()
                .AddTransient<GpsDataReceivedEventHandler>()
                .AddTransient<CompassDataReceivedEventHandler>()
                .AddTransient<IUiConfigurationRepository, UiConfigurationRepository>()
                .AddTransient<ISensorRepository, SensorRepository>()
                .AddTransient<ISensorDataRepository, SensorDataRepository>()
                .AddTransient<IStreamRepository, StreamRepository>()
                .AddTransient<UbiquitiStatsProvider>()
                .AddTransient<IGamepadProcessor<RoverMixer, RoverProcessorResult>,
                    ExponentialGamepadProcessor<RoverMixer, RoverProcessorResult>>()
                .AddTransient<IDbSeeder, DbSeeder>()
                .AddUbiquitiPoller(Configuration)
                .AddHealthChecks(Configuration)
                .AddHostedService<EventBusHostedService>()
                .AddCanOpenFileEds(new FileRepositoryConfiguration()
                    .WithMiControlPath(Path.Combine(Env.ContentRootPath, "Resources", "mcDSA - E25.json"))
                    .WithScorpioEdsPath(Path.Combine(Env.ContentRootPath, "Resources", "scorpioCAN.json"))
                );

            services.AddTransient<PdfCreator>();
            services.AddTransient<IReportBuilder, ReportBuilder>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
        {
            // Make sure the CORS middleware is ahead of SignalR.
            app.UseCors(builder =>
            {
                var origins = config.GetSection("AllowedOrigins").AsEnumerable().Select(x => x.Value).Where(x => x != null).ToArray();
                builder.WithOrigins(origins)
                    .WithMethods("GET", "POST", "PUT", "OPTIONS", "DELETE")
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            // i.e. /api/swagger/v1/swagger.json
            app.UseSwagger(o => o.RouteTemplate = "/api/swagger/{documentName}/swagger.json");

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            // i.e. /api/swagger
            app.UseSwaggerUI(o => { o.SwaggerEndpoint("/api/swagger/v1/swagger.json", "My API V1"); o.RoutePrefix = "api/swagger"; });

            app.UseExceptionHandlingMiddleware();

            // all the api routes starts with /api prefix, others should be redirected to index.html and handled by client
            // fallback to SPA routing
            app.UseClientSideFallbackRouting();

            // service index.html etc by default
            app.UseDefaultFiles();

            // servce static files from wwwroot
            app.UseStaticFiles();

            // use API routes
            app.UseRouting();

            // map api routes (health checks, signalR hub, controllers
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MainHub>("/api/hub");
                endpoints.MapHealthChecks("/api/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = WriteHealthResponse
                });
            });
        }

        private static Task WriteHealthResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("isHealthy", result.Status == HealthStatus.Healthy),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("isHealthy", pair.Value.Status == HealthStatus.Healthy),
                        new JProperty("description", pair.Value.Description)))))));

            return context.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }

    public static class StartupExtensions
    {
        public static IApplicationBuilder UseClientSideFallbackRouting(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            return app;

        }
        public static IServiceCollection AddUbiquitiPoller(this IServiceCollection services, IConfiguration config)
        {
            var enabled = config.GetValue<bool>("Ubiquiti:EnablePoller");

            if (enabled)
                services.AddHostedService<UbiquitiPollerHostedService>();

            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            var brokerHost = config["socketClient:host"];
            var brokerPort = int.Parse(config["socketClient:port"]);

            var mongoDbConnectionString = config.GetValue<string>("MongoDb:ConnectionString");

            var timeout = TimeSpan.FromSeconds(1.5);

            services.AddHealthChecks()
                .AddMongoDb(mongoDbConnectionString, name: "MongoDb", timeout: timeout )
                .AddTcpHealthCheck(options => { options.AddHost(brokerHost, brokerPort); }, "ScorpioBroker", timeout: timeout);

            return services;
        }
    }
}
