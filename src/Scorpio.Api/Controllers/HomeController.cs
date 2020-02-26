using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Scorpio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class HomeController : ControllerBase
    {
        private readonly IOptions<RabbitMqConfiguration> _rabbitConfig;
        private readonly IOptions<MongoDbConfiguration> _mongoConfig;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public HomeController(IOptions<RabbitMqConfiguration> rabbitConfig, IOptions<MongoDbConfiguration> mongoConfig, IWebHostEnvironment env, IConfiguration config)
        {
            _rabbitConfig = rabbitConfig;
            _mongoConfig = mongoConfig;
            _env = env;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sec = _config.GetSection("socketClient");

            var response = new
            {
                SwaggerDocs = "/swagger",
                Env = _env.EnvironmentName,
                Api = Assembly.GetExecutingAssembly().GetName(),
                RaabiqMqConfig = _rabbitConfig.Value,
                MongoDbConfig = _mongoConfig.Value,
                SocketClient = _config.GetSection("socketClient").AsEnumerable().Where(x => x.Value != null).ToDictionary(x => x.Key, x => x.Value)
            };

            return Ok(response);
        }
    }
}