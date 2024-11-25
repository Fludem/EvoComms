using System;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace EvoComms.Core.Database.Models
{
    public class Clocking
    {
        public int Id { get; set; }
        public int ClockingMachineId { get; set; }
        public int EmployeeId { get; set; }
        public ClockingMachine ClockingMachine { get; set; } = null!;
        public required Employee Employee { get; set; }
        public DateTime ClockedAt { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}