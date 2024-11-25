using System.Text;
using EvoComms.Core.Filesystem.Writers;
using EvoComms.Core.Models;
using EvoComms.Core.Services;
using EvoComms.Devices.ZKTeco.Models;
using EvoComms.Devices.ZKTeco.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

#pragma warning disable CS0414 // Field is assigned but its value is never used

namespace EvoComms.Devices.ZKTeco.Http.Controllers;

[ApiController]
[Microsoft.AspNetCore.Components.Route("iclock")]
public class ZkTecoController : ControllerBase
{
    private readonly IClockingWriterFactory _clockingWriterFactory;
    private readonly string _delay = "15";
    private readonly string _encrypt = "0";

    private readonly string _errorDelay = "3";
    private readonly bool _getAllLogs = true;
    private readonly ILogger<ZkTecoController> _logger;
    private readonly string _realTime = "1";
    private readonly RecordService _recordService;
    private readonly string _timeZone = "0";
    private readonly string _transFlag = "1111000000";
    private readonly string _transInterval = "1";
    private readonly string _transTimes = "00:00;14:05";
    private readonly ZkSettingsProvider _zkSettingsProvider;

    public ZkTecoController(ILogger<ZkTecoController> logger, IClockingWriterFactory clockingWriterFactory,
        ZkSettingsProvider zkSettingsProvider, RecordService recordService
    )
    {
        _logger = logger;
        _clockingWriterFactory = clockingWriterFactory;
        _zkSettingsProvider = zkSettingsProvider;
        _recordService = recordService;
    }

    [HttpGet("cdata")]
    public async Task<IActionResult> HandleInitialConnection()
    {
        try
        {
            foreach (var header in Request.Headers) _logger.LogInformation($"Header: {header.Key} = {header.Value}");
            Request.EnableBuffering();

            Request.Body.Position = 0;
            var rawRequestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            _logger.LogInformation($"Request body: {rawRequestBody}");
            // Get the device serial number from the query string
            var serialNumber = Request.Query["SN"].ToString();
            if (string.IsNullOrEmpty(serialNumber))
            {
                _logger.LogWarning("Device connection attempt without serial number");
                return BadRequest("Serial number is required");
            }

            _logger.LogInformation($"Device {serialNumber} initiated connection");

            // Construct the configuration response
            var response = new StringBuilder();
            response.AppendLine($"GET OPTION FROM:{serialNumber}");
            response.AppendLine("AttStamp=1");
            response.AppendLine("OpStamp=1");
            response.AppendLine("PhotoStamp=1");
            response.AppendLine($"ErrorDelay={_errorDelay}");
            response.AppendLine($"Delay={_delay}");
            response.AppendLine($"TransTimes={_transTimes}");
            response.AppendLine($"TransInterval={_transInterval}");
            response.AppendLine($"TransFlag={_transFlag}");
            response.AppendLine($"Realtime={_realTime}");
            response.AppendLine($"Encrypt={_encrypt}");
            response.AppendLine($"TimeZone={_timeZone}");
            _logger.LogInformation($"Response Body: {response}");
            return Content(response.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling device connection");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("devicecmd")]
    public async Task<IActionResult> HandleDeviceCommand()
    {
        try
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            _logger.LogInformation($"Received device command response: {body}");

            // The device will respond with attendance data
            // Check for successful response
            if (body.Contains("Return=0") && body.Contains("CMD=DATA"))
            {
                await ProcessAttendanceLogs(body);
                return Ok("OK");
            }

            if (body.Contains("Return="))
            {
                // Handle error codes from the device
                var errorCode = GetErrorCode(body);
                _logger.LogWarning($"Device returned error code: {errorCode}");
            }

            return Ok("OK"); // Always respond with OK to acknowledge receipt
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing device command response");
            return StatusCode(500, "Internal server error");
        }
    }

    private int GetErrorCode(string response)
    {
        try
        {
            // Extract error code from "Return=X" where X is the code
            var returnIndex = response.IndexOf("Return=", StringComparison.Ordinal);
            if (returnIndex >= 0)
            {
                var codeStart = returnIndex + 7; // length of "Return="
                var codeEnd = response.IndexOf('&', codeStart);
                if (codeEnd < 0) codeEnd = response.Length;
                var codeStr = response.Substring(codeStart, codeEnd - codeStart);
                if (int.TryParse(codeStr, out var code))
                    return code;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing device error code");
        }

        return -1;
    }


    [HttpPost("cdata")]
    public async Task<IActionResult> HandleAttendanceData()
    {
        try
        {
            using var reader = new StreamReader(Request.Body);
            var data = await reader.ReadToEndAsync();
            if (Request.Query.ContainsKey("table") && Request.Query["table"].ToString() == "ATTLOG")
            {
                // var dataStart = data.IndexOf("CMD=DATA") + 9;
                // if (dataStart < 9)
                // {
                //     _logger.LogInformation("no data found");
                // }
                // else
                // {
                var records = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                await ProcessAttendanceLogs(data);
                _logger.LogInformation($"Recieved: {records.Length} records");
                return Content("OK");
                // }
            }

            if (data.Contains("OPERLOG")) ProcessOperationLog(data);

            return Content("OK");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing attendance data");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("getrequest")]
    public IActionResult HandleGetRequest()
    {
        try
        {
            var serialNumber = Request.Query["SN"].ToString();
            if (string.IsNullOrEmpty(serialNumber))
            {
                _logger.LogWarning("Device request without serial number");
                return BadRequest("Serial number is required");
            }

            if (!_getAllLogs) return Content("OK");
            _logger.LogTrace($"Device {serialNumber} requested commands - sending attendance logs request");

            // Based on the source code, the format is "C:ID1:DATA QUERY ATTLOG"
            // The ID1 identifier is used to track this specific command
            //var response = "C:ID1:DATA QUERY ATTLOG StartTime=2024-11-01 00:00:00  EndTime=2024-11-15 00:00:00";
            //GetAllLogs = false;
            return Content("OK");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling get request");
            return StatusCode(500, "Internal server error");
        }
    }

    private async Task SaveAttendanceLogs(List<Record> records)
    {
        _logger.LogInformation("Saving attendance logs");
        var settings = await _zkSettingsProvider.LoadSettings();
        _logger.LogInformation($"Writing Clocking Files. Root Output Path: {settings.OutputPath}");
        var record = records.First();
        var serialNumber = record.DeviceSerial ?? "1";
        await _recordService.ProcessClockings(records, serialNumber, settings);
        //
        // foreach (var record in records)
        // {
        //     _logger.LogInformation($"Writing Record: {record}");
        //     var dateTime = DateTime.ParseExact(record.Time, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        //     await writer.WriteClocking(record.EmployeeId, dateTime, settings.OutputPath,
        //         record.DeviceSerialNumber ?? "1");
        // }
    }

    private async Task ProcessAttendanceLogs(string data)
    {
        try
        {
            // Find the attendance data after the command response
            _logger.LogWarning($"Device\n {data} \n sent attendance logs request");
            var records = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            _logger.LogInformation($"Log Count: {records.Length} ");
            var settings = await _zkSettingsProvider.LoadSettings();
            var writer = _clockingWriterFactory.GetWriter(settings.OutputType);
            var currentClockings = new List<Record>();
            foreach (var record in records)
            {
                var fields = record.Split('\t');
                if (fields.Length < 5) _logger.LogInformation(fields.ToString());

                var log = new AttendanceRecord
                {
                    UserId = fields[0].Trim(),
                    DateTime = DateTime.Parse(fields[1].Trim()),
                    InOutMode = "ParseInOutMode(fields[2].Trim())",
                    VerifyMode = "ParseVerifyMode(fields[3].Trim())",
                    EventCode = fields[4].Trim()
                };
                _logger.LogInformation(
                    "Retrieved attendance record: User {userId} at {time}",
                    log.UserId,
                    log.DateTime
                );

                var empId = int.Parse(log.UserId);
                var time = DateTime.Parse(fields[1].Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                var clocking = new Record
                {
                    EmployeeId = empId,
                    DeviceSerial = "1",
                    EmployeeName = "ZK",
                    Time = time
                };
                _logger.LogInformation($"Converted to record: {clocking}");
                currentClockings.Add(clocking);
            }

            await SaveAttendanceLogs(currentClockings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing attendance logs from device command");
            throw;
        }
    }

    private void ProcessOperationLog(string data)
    {
        // Implement operation log processing if needed
        _logger.LogInformation("Operation log received");
    }

    private void SaveAttendanceRecord(AttendanceRecord attendanceRecord)
    {
        _logger.LogInformation(
            $"Attendance record received for user {attendanceRecord.UserId} at {attendanceRecord.DateTime}");
    }
}