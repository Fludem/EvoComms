using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EvoComms.Core.Database.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [MaxLength(50)] public string? Name { get; set; }

        public int ClockingId { get; set; }

        public virtual ICollection<Clocking> Clockings { get; set; } = new List<Clocking>();
    }
}