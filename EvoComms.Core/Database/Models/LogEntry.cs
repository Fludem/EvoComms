using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace EvoComms.Core.Database.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        [MaxLength(50)] public required string LogLevel { get; set; }
        public string? Message { get; set; } = "NA";
        public string? Exception { get; set; }

        [MaxLength(150)] public string? Logger { get; set; }
    }
}