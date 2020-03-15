using Microsoft.Extensions.Logging;
using Scorpio.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Scorpio.Api.DataAccess.Seeding
{
    public class DbSeeder : IDbSeeder
    {
        private readonly ILogger<DbSeeder> _logger;
        private readonly ISensorRepository _sensorRepository;
        private readonly IUiConfigurationRepository _uiConfigurationRepository;

        public DbSeeder(ILogger<DbSeeder> logger,
            ISensorRepository sensorRepository,
            IUiConfigurationRepository uiConfigurationRepository)
        {
            _logger = logger;
            _sensorRepository = sensorRepository;
            _uiConfigurationRepository = uiConfigurationRepository;
        }

        public async Task Seed()
        {
            _logger.LogInformation("Seeder checking for DB existence...");

            await SeedSensors();
            await SeedUiConfiguration();
        }

        private async Task SeedSensors()
        {
            var existingSensors = await _sensorRepository.GetAllAsync();

            if (!existingSensors.Any())
            {
                _logger.LogWarning("No sensors found in database!");
                _logger.LogInformation("Seeding sensors....");

                await _sensorRepository.CreateAsync(new Sensor
                {
                    Name = "Voltage",
                    SensorKey = "voltage",
                    Unit = "[V]"
                });

                await _sensorRepository.CreateAsync(new Sensor
                {
                    Name = "Current",
                    SensorKey = "current",
                    Unit = "[A]"
                });


                await _sensorRepository.CreateAsync(new Sensor
                {
                    Name = "Temperature",
                    SensorKey = "temperature",
                    Unit = "[deg C]"
                });

                await _sensorRepository.CreateAsync(new Sensor
                {
                    Name = "Humidity",
                    SensorKey = "humidity",
                    Unit = "[%]"
                });

                await _sensorRepository.CreateAsync(new Sensor
                {
                    Name = "GPS",
                    SensorKey = "gps",
                    Unit = "[lat, lon]"
                });

                _logger.LogInformation("Seeding sensors done!");
            }
        }

        private async Task SeedUiConfiguration()
        {
            var existingConfigs = await _uiConfigurationRepository.GetAllAsync();

            if (!existingConfigs.Any(x => UiConfiguration.ConfigType.Page.Equals(x.Type)))
            {
                _logger.LogWarning("No UI Configuration found in database!");
                _logger.LogInformation("Seeding UI Configurations....");

                var home = await _uiConfigurationRepository.CreateAsync(new UiConfiguration
                {
                    ParentId = "0",
                    Name = "home",
                    Data = "{}",
                    Type = UiConfiguration.ConfigType.Page
                });

                await _uiConfigurationRepository.CreateAsync(new UiConfiguration
                {
                    ParentId = home.Id,
                    Name = "Rover battery",
                    Data = "{\"type\":\"BatteryWidget\",\"props\":{\"widgetTtype\":\"BatteryWidget\",\"widgetTitle\":\"Rover battery\"}}",
                    Type = UiConfiguration.ConfigType.Member
                });

                await _uiConfigurationRepository.CreateAsync(new UiConfiguration
                {
                    ParentId = home.Id,
                    Name = "Ubiuqiti stats",
                    Data = "{\"type\":\"UbiquitiWidget\",\"props\":{\"widgetTtype\":\"UbiquitiWidget\",\"widgetTitle\":\"Ubiuqiti stats\"}}",
                    Type = UiConfiguration.ConfigType.Member
                });

                await _uiConfigurationRepository.CreateAsync(new UiConfiguration
                {
                    ParentId = home.Id,
                    Name = "Gamepad 0",
                    Data = "{\"type\":\"GamepadAnalogs\",\"props\":{\"widgetTtype\":\"GamepadAnalogs\",\"widgetTitle\":\"Gamepad 0\",\"gamepadIndex\":\"0\"}}",
                    Type = UiConfiguration.ConfigType.Member
                });

                await _uiConfigurationRepository.CreateAsync(new UiConfiguration
                {
                    ParentId = home.Id,
                    Name = "Temperature",
                    Data = "{\"type\":\"Chart\",\"props\":{\"widgetTtype\":\"Chart\",\"widgetTitle\":\"Temperature\",\"sensorKey\":\"temperature\"}}",
                    Type = UiConfiguration.ConfigType.Member
                });


                await _uiConfigurationRepository.CreateAsync(new UiConfiguration
                {
                    ParentId = home.Id,
                    Name = "Last measured humidity",
                    Data = "{\"type\":\"StatisticWidget\",\"props\":{\"widgetTtype\":\"StatisticWidget\",\"widgetTitle\":\"Last measured humidity\",\"sensorKey\":\"humidity\"}}",
                    Type = UiConfiguration.ConfigType.Member
                });

                _logger.LogInformation("Seeding UI Configurations done!");
            }
        }
    }
}
