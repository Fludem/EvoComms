using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using EvoComms.Core.Models;
using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Messages.Attributes;
using EvoComms.Devices.Timy.Messages.Incoming;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.Outgoing.Requests;

public class GetAllLogCommand : BaseServerRequest
{
    [SetsRequiredMembers]
    public GetAllLogCommand(DateTime startDate, DateTime endDate)
    {
        Command = "getalllog";
        EndDate = endDate.ToString("yyyy-MM-dd");
        StartDate = startDate.ToString("yyyy-MM-dd");
    }

    [JsonPropertyName("stn")] public bool FirstPacket { get; set; } = true;
    [JsonPropertyName("from")] public string StartDate { get; set; }
    [JsonPropertyName("to")] public string EndDate { get; set; }
}

public class GetAllLogResponse : BaseServerResponse
{
    [JsonPropertyName("sn")] public required string SerialNumber { get; set; } = "1";
    [JsonPropertyName("count")] public int TotalRecordsCount { get; set; }
    [JsonPropertyName("from")] public int FromDate { get; set; }
    [JsonPropertyName("to")] public int EndDate { get; set; }

    public required List<Record> Records { get; set; }

    public string Response()
    {
        var response = JsonConvert.SerializeObject(new ContinueAllLog());
        return response;
    }

    public List<Record> GetRecords()
    {
        foreach (var clocking in
                 Records) clocking.DeviceSerialNumber = "NA";

        return Records;
    }
}

[TimyResponseHandler("getalllog")]
public class GetAllLogHandler : BaseITimyMessageHandler
{
    public GetAllLogHandler(ILogger<GetAllLogHandler> logger,
        RecordService recordService,
        TimySettingsProvider timySettingsProvider) : base(logger, recordService, timySettingsProvider)
    {
    }

    public override async Task Handle(WebSocketSession session, string message)
    {
        try
        {
            var getAllLogResponse = JsonConvert.DeserializeObject<GetAllLogResponse>(message) ??
                                    throw new InvalidOperationException();
            _logger.LogDebug(
                $"Serialized Get All Log Response. Serial - {getAllLogResponse.SerialNumber} | Total Count - {getAllLogResponse.TotalRecordsCount} | Records Index Start: {getAllLogResponse.FromDate} | Records Index End: {getAllLogResponse.EndDate}");

            if (getAllLogResponse.TotalRecordsCount > getAllLogResponse.EndDate)
            {
                _logger.LogInformation(
                    $"Device {getAllLogResponse.SerialNumber} has {getAllLogResponse.TotalRecordsCount - getAllLogResponse.EndDate} clockings left to collect. Clockings received so far: {getAllLogResponse.EndDate} out of {getAllLogResponse.TotalRecordsCount}");
                await session.SendAsync(getAllLogResponse.Response());
            }

            else
            {
                _logger.LogInformation(
                    $"Timy: Device {getAllLogResponse.SerialNumber} has finished sending all clockings. Total Clockings: {getAllLogResponse.TotalRecordsCount}");
            }

            var settings = await _settingsProvider.LoadSettings();
            await RecordService.ProcessClockings(getAllLogResponse.GetRecords(), getAllLogResponse.SerialNumber,
                settings);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"Error Deserializing Message For Device On IP {session.RemoteEndPoint}. Error: {e.Message} - {e.InnerException}");
        }
    }
}

public class ContinueAllLog
{
    [JsonPropertyName("cmd")] public string Command { get; set; } = "getalllog";
    [JsonPropertyName("stn")] public bool FirstPacket { get; set; }
}