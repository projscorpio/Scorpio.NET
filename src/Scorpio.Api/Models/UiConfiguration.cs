using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Scorpio.Api.Models
{
    public class UiConfiguration : EntityBase
    {
        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [Required]
        [RegularExpression("^page$|^member$", ErrorMessage = "Type must be either 'page' or 'member")] 
        [JsonProperty("type")]
        public string Type { get; set; }

        [Required]
        [MinLength(2)]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [JsonProperty("data")]
        public string Data { get; set; }

        public override string ToString() => Name ?? string.Empty;

        public static class ConfigType
        {
            public static string Page = "page";
            public static string Member = "member";
        }
    }
}
