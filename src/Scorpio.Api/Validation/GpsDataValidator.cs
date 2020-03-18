using Matty.Framework.Validation;
using Newtonsoft.Json;
using Scorpio.Api.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Scorpio.Api.Validation
{
    public class GpsDataValidator : ISensorDataValidator
    {
        public string SensorKey => "gps";

        public void Validate(SensorData sensorData)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<GpsData>(sensorData.Value);
                data.Validate();
            }
            catch (JsonSerializationException ex)
            {
                throw new ValidationException(ex.Message);
            }
            catch (JsonReaderException ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
    }

    /// <summary>
    /// Model class
    /// </summary>
    public class GpsData : ValidatableParamBase<GpsData>
    {
        [Required(ErrorMessage = "Field 'latitude' is required")]
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [Required(ErrorMessage = "Field 'longitude' is required")]
        [JsonProperty("longitude")]
        public double? Longitude { get; set; }
    }
}