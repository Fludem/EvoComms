using System.Text.Json;
using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Messages.Attributes;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Logging;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.Incoming.Commands.Reg;

[TimyCommandHandler("reg")]
public class RegisterHandler(
    ILogger<RegisterHandler> logger,
    RecordService recordService,
    TimySettingsProvider timySettingsProvider)
    : BaseMessageHandler(logger, recordService, timySettingsProvider)
{
    public override async Task Handle(WebSocketSession session, string message)
    {
        _logger.LogInformation(message);
        try
        {
            var regCommand = JsonSerializer.Deserialize<RegCommand>(message) ??
                             throw new ArgumentNullException(
                                 "JsonSerializer.Deserialize<RegCommand>(message)");
            _logger.LogInformation(
                $"Session Started With Device. ID: {session.SessionID} IP: {session.RemoteEndPoint} Serial: {regCommand.sn}");
            _logger.LogInformation(
                $"Timy: New Device Connection. Serial - {regCommand.sn} | Current Time On Terminal - {regCommand.deviceInfo.DeviceTime} | New Records: {regCommand.deviceInfo.NewClockingCount} | Total Records: {regCommand.deviceInfo.TotalClockingCount}");
            await session.SendAsync(regCommand.Response());
            // if (_isFirst)
            // {
            //     _logger.LogInformation($"Is First for {_regCommand.sn} is true");
            //     await Task.Delay(50);
            //     string alllog = JsonSerializer.Serialize(new GetAllLogCommand());
            //     _logger.LogInformation($"Getting All Logs With Command {alllog}");
            //     await session.SendAsync(alllog);
            //     _isFirst = false;
            // }
        }
        catch (Exception e)
        {
            _logger.LogError("Timy: Error Deserializing Message: " + e.Message);
        }
    }
}