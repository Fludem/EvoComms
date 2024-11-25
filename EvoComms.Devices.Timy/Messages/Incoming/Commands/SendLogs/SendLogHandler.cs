using System.Text.Json;
using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Messages.Attributes;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Logging;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.Incoming.Commands.SendLogs;

[TimyCommandHandler("sendlog")]
public class SendLogHandler : BaseITimyMessageHandler
{
    public SendLogHandler(
        ILogger<SendLogHandler> logger,
        RecordService recordService,
        TimySettingsProvider settingsProvider)
        : base(logger, recordService, settingsProvider)
    {
    }

    public override async Task Handle(WebSocketSession session, string message)
    {
        try
        {
            var sendLogCommand = JsonSerializer.Deserialize<SendLogCommand>(message) ??
                                 throw new InvalidOperationException();
            _logger.LogInformation(
                $"Received SendLog Command from device {sendLogCommand.SerialNumber} on IP {session.RemoteEndPoint} Session {session.SessionID}");
            var settings = await _settingsProvider.LoadSettings();
            await RecordService.ProcessClockings(sendLogCommand.GetRecords(), sendLogCommand.SerialNumber, settings);
            await session.SendAsync(sendLogCommand.Response());
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Error handling SendLog command");
        }
    }
}