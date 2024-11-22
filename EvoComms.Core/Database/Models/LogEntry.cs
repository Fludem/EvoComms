using System;

namespace EvoComms.Core.Database.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string? Exception { get; set; }

        public string? Logger { get; set; }
    }
}