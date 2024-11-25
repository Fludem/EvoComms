﻿using System;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace EvoComms.Core.Database.Models
{
    public class Clocking
    {
        public int Id { get; set; }
        public int ClockingMachineId { get; set; }
        public int EmployeeId { get; set; }
        public ClockingMachine ClockingMachine { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public DateTime ClockedAt { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}