using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using EvoComms.Core.Database.Models;
using EvoComms.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace EvoComms.Core.Filesystem.Writers.InTime
{
    public class InTimeWriter(ILogger<InTimeWriter> logger)
        : IClockingFileWriter
    {
        public string fileEnd = "MEM = 0000\r\nMODE EXIT\r\nInfotime OK H\r\n\"\r\n";

        public string fileStart =
            "\">*1\r\nTU 12/07/22 03:55\r\nMODE 2 SELECTED:\r\nMODE 3 SELECTED:\r\nSERIAL NUMBER:- 123456\r\nSIMPLE PRINT MEMORY:-\r\nMEM = 0000\r\n";

        public async Task WriteFromModels(List<Clocking> clockings)
        {
            string nowFormatted = DateTime.Now.ToString("ddMMyyHHmmss");
            int randomInt = new Random().Next(2000);
            string filepath = Path.Combine("C:/temp", $"evocomms{nowFormatted}{randomInt}.dat");
            logger.LogInformation($"Writing Clocking Files to: {filepath}");
            try
            {
                await using (StreamWriter writer = new(filepath, false))
                {
                    // Write file start
                    await writer.WriteAsync(fileStart);

                    logger.LogTrace($"Writing {clockings.Count} clockings to file");
                    foreach (Clocking clocking in clockings)
                    {
                        logger.LogInformation(
                            $"Writing record clocking from device: {clocking.ClockingMachine.SerialNumber} with ID: {clocking.Id}");
                        string clockingFileEntry = ConvertClockingToFileEntry(clocking);
                        await writer.WriteAsync(clockingFileEntry);
                    }

                    logger.LogTrace("Writing file end");
                    await writer.WriteAsync(fileEnd);
                }

                logger.LogInformation($"Clocking Writer Module: Saved File at: {filepath}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error Writing Clocking Files: {ex.Message}");
                logger.LogError($"Failed Writing: {clockings.Count} records");
            }
        }

        public async Task WriteClocking(int employeeId, DateTime clockingTime, string filepath, string serialNumber)
        {
            string nowFormatted = DateTime.Now.ToString("dd_MM_yy_HHmmss");
            int randomInt = new Random().Next(20000);
            string outputPath = Path.Combine(filepath, $"evocomms{nowFormatted}{randomInt}.dat");
            try
            {
                logger.LogInformation($"Attempting to write Clocking File to: {outputPath}");
                await using (StreamWriter writer = new(outputPath, false))
                {
                    // Write file start
                    await writer.WriteAsync(fileStart);
                    logger.LogInformation($"Writing clocking to file - Emp ID: {employeeId}. Time: {clockingTime}");
                    string clockingFileEntry = MakeFileLine(employeeId, clockingTime);
                    await writer.WriteAsync(clockingFileEntry);
                    await writer.WriteAsync(fileEnd);
                }

                logger.LogInformation($"Clocking Writer Module: Saved File at: {filepath}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error Writing Clocking Files: {ex.Message}");
            }
        }

        private string MakeFileLine(int employeeId, DateTime clockTime)
        {
            string userid = employeeId.ToString().PadLeft(8, '0');
            string dayMonthYear = clockTime.ToString("ddMMyy", CultureInfo.InvariantCulture);
            string formattedTime = clockTime.ToString("HHmm", CultureInfo.InvariantCulture);
            return $" {userid} MO {dayMonthYear}  {formattedTime}{Environment.NewLine}";
        }

        private string ConvertClockingToFileEntry(Clocking clocking)
        {
            string userid = clocking.Employee.ClockingId.ToString().PadLeft(8, '0');
            string dayMonthYear = clocking.ClockedAt.ToString("ddMMyy", CultureInfo.InvariantCulture);
            string formattedTime = clocking.ClockedAt.ToString("HHmm", CultureInfo.InvariantCulture);
            return $" {userid} MO {dayMonthYear}  {formattedTime}{Environment.NewLine}";
        }
    }
}