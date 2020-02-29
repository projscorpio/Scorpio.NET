using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Scorpio.CanOpenEds
{
    public class SendCanOpenObjectParam : IValidatableObject
    {
        [Required]
        [RegularExpression("[0-9a-fA-F,x]+", ErrorMessage = "Index must be hexadecimal")]
        [JsonProperty("index")]
        public string Index { get; set; }

        [Required]
        [RegularExpression("[0-9a-fA-F,x]+", ErrorMessage = "SubIndex must be hexadecimal")]
        [JsonProperty("subIndex")]
        public string SubIndex { get; set; }

        [Required]
        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [Required]
        [JsonProperty("value")]
        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var allowedDataTypes = new List<string>
            {
                "UNKNOWN"                    ,
                "BOOLEAN"                    ,
                "INTEGER8"                   ,
                "INTEGER16"                  ,
                "INTEGER32"                  ,
                "UNSIGNED8"                  ,
                "UNSIGNED16"                 ,
                "UNSIGNED32"                 ,
                "REAL32"                     ,
                "VISIBLE_STRING"             ,
                "OCTET_STRING"               ,
                "UNICODE_STRING"             ,
                "TIME_OF_DAY"                ,
                "TIME_DIFFERENCE"            ,
                "DOMAIN"                     ,
                "INTEGER24"                  ,
                "REAL64"                     ,
                "INTEGER40"                  ,
                "INTEGER48"                  ,
                "INTEGER56"                  ,
                "INTEGER64"                  ,
                "UNSIGNED24"                 ,
                "UNSIGNED40"                 ,
                "UNSIGNED48"                 ,
                "UNSIGNED56"                 ,
                "UNSIGNED64"                 ,
                "PDO_COMMUNICATION_PARAMETER",
                "PDO_MAPPING"                ,
                "SDO_PARAMETER"              ,
                "IDENTITY"
            };

            if (!allowedDataTypes.Any(x => string.Equals(x, DataType)))
            {
                var validValues = string.Join(",", allowedDataTypes);
                yield return new ValidationResult($"dataType is invalid, must be one of: " + validValues);
            }

            switch (DataType)
            {
                case "INTEGER8":
                    if (!sbyte.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "INTEGER16":
                    if (!Int16.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "INTEGER32":
                    if (!Int32.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "INTEGER64":
                    if (!Int64.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "UNSIGNED8":
                    if (!byte.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "UNSIGNED16":
                    if (!UInt16.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "UNSIGNED32":
                    if (!UInt32.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "UNSIGNED64":
                    if (!UInt64.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "BOOLEAN":
                    if (!string.Equals(Value, "true", StringComparison.InvariantCultureIgnoreCase)
                        || !string.Equals(Value, "false", StringComparison.InvariantCultureIgnoreCase))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "REAL32":
                    if (!float.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;

                case "REAL64":
                    if (!double.TryParse(Value, out _))
                        yield return new ValidationResult($"Value is in invalid range to chosen dataType");
                    break;
            }
        }
    }
}
