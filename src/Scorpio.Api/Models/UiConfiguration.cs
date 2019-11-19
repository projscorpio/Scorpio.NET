﻿using System.ComponentModel.DataAnnotations;

namespace Scorpio.Api.Models
{
    public class UiConfiguration : EntityBase
    {
        public string ParentId { get; set; }

        [Required]
        public int Type { get; set; }

        public string Name { get; set; }

        public string Data { get; set; }

        public override string ToString() => Name ?? string.Empty;
    }
}
