using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EvoComms.Core.Database.Models;

namespace EvoComms.Core.Interfaces
{
    public interface IClockingFileWriter
    {
        Task WriteFromModels(List<Clocking> clockings);
        Task WriteClocking(int employeeId, DateTime clockingTime, string filepath, string serialNumber);
    }
}