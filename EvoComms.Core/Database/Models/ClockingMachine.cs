using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EvoComms.Core.Database.Models
{
    public class ClockingMachine
    {
        public int Id { get; set; }

        [MaxLength(80)] public required string Name { get; set; }

        [MaxLength(80)] public string? SerialNumber { get; set; }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public virtual ICollection<Clocking> Clockings { get; set; } = new List<Clocking>();
    }
}