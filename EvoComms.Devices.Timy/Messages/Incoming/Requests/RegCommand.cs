using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Messages.Attributes;
using EvoComms.Devices.Timy.Messages.Outgoing.Requests;
using EvoComms.Devices.Timy.Models;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Logging;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.Incoming.Requests;

public class RegRequest : BaseDeviceRequest
{
    [JsonPropertyName("devinfo")] public required DeviceInfo DeviceInfo { get; set; }
    [JsonPropertyName("sn")] public required string SerialNumber { get; set; } = "1";

    public string Response()
    {
        var response = JsonSerializer.Serialize(new RegisterServerResponse(true));
        return response;
    }
}

public class RegisterServerResponse : BaseServerResponse
{
    [SetsRequiredMembers]
    public RegisterServerResponse(bool success, string? serverTime = null)
    {
        Command = "reg";
        Success = success;
        ServerTime = serverTime ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    [JsonPropertyName("cloudtime")] public required string ServerTime { get; set; }
    [JsonPropertyName("nosenduser")] public bool DisabledNewUserSending { get; set; } = true;
}

[TimyCommandHandler("reg")]
public class RegisterHandler(
    ILogger<RegisterHandler> logger,
    RecordService recordService,
    TimySettingsProvider timySettingsProvider)
    : BaseITimyMessageHandler(logger, recordService, timySettingsProvider)
{
    public override async Task Handle(WebSocketSession session, string message)
    {
        _logger.LogInformation(message);
        try
        {
            var regCommand = JsonSerializer.Deserialize<RegRequest>(message) ??
                             throw new ArgumentNullException(
                                 "JsonSerializer.Deserialize<RegCommand>(message)");
            _logger.LogInformation(
                $"Session Started With Device. ID: {session.SessionID} IP: {session.RemoteEndPoint} Serial: {regCommand.SerialNumber}");
            _logger.LogInformation(
                $"Timy: New Device Connection. Serial - {regCommand.SerialNumber} | Current Time On Terminal - {regCommand.DeviceInfo.DeviceTime} | New Records: {regCommand.DeviceInfo.NewClockingCount} | Total Records: {regCommand.DeviceInfo.TotalClockingCount}");
            await session.SendAsync(regCommand.Response());
        }
        catch (Exception e)
        {
            _logger.LogError("Timy: Error Deserializing Message: " + e.Message);
        }
    }

    public async Task GetLogs(RegRequest regRequest, WebSocketSession session, DateTime fromDate, DateTime toDate)
    {
        _logger.LogInformation($"Getting All Logs From Terminal {regRequest.SerialNumber}");
        var alllog = JsonSerializer.Serialize(new GetAllLogCommand(fromDate, toDate));
        await session.SendAsync(alllog);
    }
}