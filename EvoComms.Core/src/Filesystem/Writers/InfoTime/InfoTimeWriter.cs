using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using EvoComms.Core.Database.Models;
using EvoComms.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace EvoComms.Core.Filesystem.Writers.InfoTime
{
    public class InfoTimeWriter(ILogger<InfoTimeWriter> logger)
        : IClockingFileWriter
    {
        private string? _recentSerial;

        public async Task WriteFromModels(List<Clocking> clockings)
        {
            string nowFormatted = DateTime.Now.ToString("dd_MMMM_yyyy_HH_mm");
            foreach (Clocking clocking in clockings)
            {
                if (clocking.ClockingMachine.SerialNumber != null)
                {
                    string terminalFolder = GetTerminalFolder("C:/temp", clocking.ClockingMachine.SerialNumber);
                    string filepath = Path.Combine(terminalFolder, $"A300_Clockings_{nowFormatted}.csv");
                    logger.LogInformation(
                        $"Writing Clocking File for {clocking.ClockingMachine.SerialNumber} to: {filepath}");
                    await WriteToFile(filepath, clocking);
                }
                else
                {
                    logger.LogWarning(
                        "Device didn't send serial number with clocking record. Writing clocking to root folder and skipping terminal specific folder");
                    string filePath = Path.Combine("C:/temp", $"A300_Clockings_{nowFormatted}.csv");
                    await WriteToFile(filePath, clocking);
                }
            }
        }

        public async Task WriteClocking(int employeeId, DateTime clockingTime, string filepath, string serialNumber)
        {
            logger.LogInformation("Infotime Writer WriteClocking Called.");
            string nowFormatted = DateTime.Now.ToString("dd_MM_yy_HHmmss");
            string terminalFolder = GetTerminalFolder(serialNumber, filepath);
            string outputPath = Path.Combine(terminalFolder, $"A300_Clockings_{nowFormatted}.csv");
            logger.LogInformation($"Writing Clocking File for {serialNumber} to: {filepath}");
            await WriteToFile(outputPath, employeeId, clockingTime);
        }

        private async Task WriteToFile(string filepath, Clocking clocking)
        {
            try
            {
                await using StreamWriter writer = new(filepath, true);
                string clockingFileEntryString = ConvertClockingToFileEntry(clocking);
                await writer.WriteAsync(clockingFileEntryString);
            }
            catch (Exception ex)
            {
                logger.LogWarning(
                    $"Error Writing Clocking for ClockingID: {clocking.Id} Failed. Employee Clocking ID: {clocking.Employee.ClockingId}. Name {clocking.Employee.Name ?? "Not Known"}. Clocking Time: {clocking.ClockedAt} received at {clocking.ReceivedAt}: {ex.Message}");
            }
        }

        private async Task WriteToFile(string filepath, int employeeId, DateTime clockingTime)
        {
            logger.LogInformation("Attempting File Write");
            try
            {
                await using StreamWriter writer = new(filepath, true);
                string clockingFileEntryString = MakeFileLine(employeeId, clockingTime);
                await writer.WriteAsync(clockingFileEntryString);
            }
            catch (Exception ex)
            {
                logger.LogWarning(
                    $"Error Writing Clocking for ClockingID: {employeeId} Failed. Clocking Time: {clockingTime}: {ex.Message}");
            }
        }

        private string ConvertClockingToFileEntry(Clocking clocking)
        {
            string infoTimeDateFormat = clocking.ClockedAt.ToString("dd/MM/yyyy,HH:mm:ss");
            logger.LogInformation("Created Entry: C,," + clocking.Employee.ClockingId + ",," + infoTimeDateFormat);
            return "C,," + clocking.Employee.ClockingId + ",," + infoTimeDateFormat + Environment.NewLine;
        }

        private string MakeFileLine(int employeeId, DateTime clockTime)
        {
            string infoTimeDateFormat = clockTime.ToString("dd/MM/yyyy,HH:mm:ss");
            return "C,," + employeeId + ",," + infoTimeDateFormat + Environment.NewLine;
        }

        private string GetTerminalFolder(string serialNumber, string filepath)
        {
            string terminalFolder = Path.Combine(filepath, serialNumber);
            if (serialNumber != _recentSerial && !Directory.Exists(terminalFolder))
            {
                logger.LogInformation($"Creating the terminal folder for {serialNumber} at {terminalFolder}");
                Directory.CreateDirectory(terminalFolder);
                _recentSerial = serialNumber;
            }

            return terminalFolder;
        }
    }
}