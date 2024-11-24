using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using EvoComms.Core.Database;
using EvoComms.Core.Database.Models;
using EvoComms.Core.Filesystem.Settings;
using EvoComms.Core.Filesystem.Writers;
using EvoComms.Core.Interfaces;
using EvoComms.Core.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EvoComms.Core.Services
{
    public class RecordService(
        ILogger<RecordService> logger,
        IClockingWriterFactory clockingWriterFactory,
        AppDbContext dbContext)
    {
        private readonly IClockingWriterFactory _clockingWriterFactory = clockingWriterFactory;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<RecordService> _logger = logger;

        public async Task<List<Clocking>> ProcessClockings(
            List<Record> records, string deviceSerialNumber, ModuleSettings settings)
        {
            List<Clocking> processedClockings = new();
            IClockingFileWriter writer = _clockingWriterFactory.GetWriter(settings.OutputType);

            foreach (Record record in records)
            {
                _logger.LogInformation($"Processing Clocking Record: {record.FormatClocking()}");
                Clocking clocking = await AddClockingToDb(record);
                processedClockings.Add(clocking);
                DateTime dateTime =
                    DateTime.ParseExact(record.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                await writer.WriteClocking(record.EmployeeId, dateTime, settings.OutputPath,
                    deviceSerialNumber ?? record.DeviceSerialNumber ?? "NA");
            }

            return processedClockings;
        }


        private async Task<ClockingMachine> GetClockingMachine(string serialNumber)
        {
            ClockingMachine? clockingMachine = await _dbContext.ClockingMachines
                .FirstOrDefaultAsync(cm => cm.SerialNumber == serialNumber);

            if (clockingMachine == null)
            {
                clockingMachine = new ClockingMachine { Name = serialNumber, SerialNumber = serialNumber };
                await _dbContext.ClockingMachines.AddAsync(clockingMachine);
                await _dbContext.SaveChangesAsync();
            }

            return clockingMachine;
        }

        public async Task<Clocking> AddClockingToDb(Record record)
        {
            ClockingMachine clockingMachine = await GetClockingMachine(record.DeviceSerialNumber ?? "Unknown");
            Employee employee = await GetEmployee(record);
            Clocking clocking = new()
            {
                EmployeeId = employee.Id,
                ClockingMachineId = clockingMachine.Id,
                ClockedAt = DateTime.ParseExact(record.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                ReceivedAt = DateTime.Now
            };
            await _dbContext.Clockings.AddAsync(clocking);
            await _dbContext.SaveChangesAsync();
            return clocking;
        }

        private async Task<Employee> GetEmployee(Record record)
        {
            Employee? employee =
                await _dbContext.Employees.FirstOrDefaultAsync(emp => emp.ClockingId == record.EmployeeId);
            if (employee == null)
            {
                employee = new Employee { Name = record.EmployeeName, ClockingId = record.EmployeeId };
                await _dbContext.Employees.AddAsync(employee);
                await _dbContext.SaveChangesAsync();
            }

            return employee;
        }
    }
}