using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using EvoComms.Core.Database.Models;
using EvoComms.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace EvoComms.Core.Filesystem.Writers.BioTime
{
    public class BioTimeWriter(ILogger<BioTimeWriter> logger)
        : IClockingFileWriter
    {
        private string? _recentSerial;

        public async Task WriteFromModels(List<Clocking> clockings)
        {
            string nowFormatted = DateTime.Now.ToString("dd_MMMM_yyyy_HH_mm_ss");
            string filepath = Path.Combine("C:/temp", $"A300_Clockings_{nowFormatted}.csv");
            logger.LogInformation($"Writing Clocking {clockings.Count} Files to: {filepath} ");
            try
            {
                using (StreamWriter writer = new(filepath, true))
                {
                    foreach (Clocking clocking in clockings)
                    {
                        string clockingFileEntry = await ConvertClockingToFileEntry(clocking);
                        await writer.WriteAsync(clockingFileEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error Writing Clocking Files: {ex.Message}");
            }
        }

        public async Task WriteClocking(int employeeId, DateTime clockingTime, string filepath, string serialNumber)
        {
            string nowFormatted = DateTime.Now.ToString("dd_MM_yy_HHmmss");
            string outputPath = Path.Combine(filepath, $"A300_Clockings_{nowFormatted}.csv");
            if (serialNumber != null)
            {
                string terminalFolder = await GetTerminalFolder(serialNumber, filepath);
                outputPath = Path.Combine(terminalFolder, $"A300_Clockings_{nowFormatted}.csv");
                logger.LogInformation($"Writing Clocking File for {serialNumber} to: {outputPath}");
            }
            else
            {
                logger.LogWarning(
                    "Device didn't send serial number with clocking record. Writing clocking to root folder and skipping terminal specific folder");
            }

            await WriteToFile(outputPath, employeeId, clockingTime);
        }

        private async Task WriteToFile(string filepath, int employeeId, DateTime clockingTime)
        {
            logger.LogInformation("BioTime Writer Attempting To Write Clocking Line");
            try
            {
                await using StreamWriter writer = new(filepath, true);
                string clockingFileEntryString = await MakeFileLine(employeeId, clockingTime);
                await writer.WriteAsync(clockingFileEntryString);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"Error Writing Clocking for ClockingID: {employeeId} Failed. Clocking Time: {clockingTime}: {ex.Message}");
            }
        }

        private async Task<string> MakeFileLine(int employeeId, DateTime clockTime)
        {
            string bioDate = clockTime.ToString("dd/MM/yyyy HH:mm:ss");
            return "12345678," + employeeId + "," + bioDate + ",1" + Environment.NewLine;
        }

        public async Task<string> ConvertClockingToFileEntry(Clocking clocking)
        {
            string infoDate = clocking.ClockedAt.ToString("dd/MM/yyyy HH:mm:ss");
            logger.LogInformation(
                "Formatted Clocking: 12345678," + clocking.Employee.ClockingId + "," + infoDate + ",1");
            return "12345678," + clocking.Employee.ClockingId + "," + infoDate + ",1" + Environment.NewLine;
        }

        private async Task<string> GetTerminalFolder(string serialNumber, string outputPath)
        {
            string terminalFolder = Path.Combine(outputPath, serialNumber);
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