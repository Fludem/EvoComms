using System.ComponentModel.DataAnnotations;

namespace EvoComms.Core.Database.Models
{
    public class ClockingMachineType
    {
        public int Id { get; set; }

        [MaxLength(25)] public required string Name { get; set; }

        [MaxLength(50)] public string? Description { get; set; }
    }
}