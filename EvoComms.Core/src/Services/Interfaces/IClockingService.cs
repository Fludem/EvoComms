using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EvoComms.Core.Database.Models;

namespace EvoComms.Core.Services.Interfaces
{
    public interface IClockingService
    {
        /// <summary>
        ///     Processes an attendance record from a ZKTeco device and creates a clocking entry
        /// </summary>
        /// <param name="empClockingId">The ID used by the employee on the clocking machines</param>
        /// <param name="serialNumber">The device serial number</param>
        /// <returns>The created clocking entry</returns>
        Task<Clocking> ProcessAttendanceRecord(int empClockingId, string? employeeName, DateTime? clockingTime,
            string serialNumber);

        /// <summary>
        ///     Gets all clockings for a specific employee within a date range
        /// </summary>
        Task<IEnumerable<Clocking>> GetEmployeeClockings(int employeeId, DateTime startDate, DateTime endDate);

        /// <summary>
        ///     Gets an employee's latest clocking
        /// </summary>
        Task<Clocking?> GetEmployeeLatestClocking(int employeeId);

        /// <summary>
        ///     Gets all clockings from a specific device
        /// </summary>
        Task<IEnumerable<Clocking>> GetDeviceClockings(string serialNumber, DateTime startDate, DateTime endDate);
    }
}