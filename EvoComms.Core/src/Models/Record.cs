using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Com.FirstSolver.Splash;

using NLog;

namespace EvoComms.Core.Models
{
    public class Record
    {
        [JsonPropertyName("enrollid")] public int EmployeeId { get; set; }
        [JsonPropertyName("name")] public string EmployeeName { get; set; }
        [JsonPropertyName("time")] public string Time { get; set; }
        public int mode { get; set; }
        public int inout { get; set; }
        public int @event { get; set; }

        [JsonIgnore] public string? DeviceSerialNumber { get; set; }

        public string FormatClocking()
        {
            return $"Clocking ID: {EmployeeId}, Name: {EmployeeName}, Time: {Time}";
        }


        public string FormattedTime()
        {
            DateTime dateTime = DateTime.ParseExact(Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return dateTime.ToString("dd/MM/yyyy  HH:mm:ss");
        }

        public static Task<Record> FromTimyUsbLine(string line)
        {
            string[] parts = line.Split('\t');
            Logger logger = LogManager.GetCurrentClassLogger();
            DateTime ParsedDateObject =
                DateTime.ParseExact(parts[6].Trim(), "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string formattedDate = ParsedDateObject.ToString("yyyy-MM-dd HH:mm:ss");
            return Task.FromResult(new Record
            {
                EmployeeId = int.Parse(parts[2]),
                EmployeeName = parts[3].Trim(),
                Time = formattedDate,
                mode = 1,
                inout = 1,
                @event = 1,
                DeviceSerialNumber = "USB"
            });
        }

        public static async Task<List<Record>> FromTimyUsbFile(string usbTextData)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            List<Record> records = new();
            IEnumerable<string> lines = usbTextData.Split('\n')
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line));
            
            foreach (string line in lines)
            {
                Record currentRecord = await FromTimyUsbLine(line);
                records.Add(currentRecord);
            }
            logger.Info($"Parsed {records.Count} records from USB");
            return records;
        }
    }
}