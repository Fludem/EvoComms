using System.Text.Json;
using System.Text.Json.Serialization;
using EvoComms.Core.Models;
using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Messages.Attributes;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Logging;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.Incoming.Requests;

public class SendLog : BaseDeviceRequest
{
    [JsonConstructor]
    public SendLog()
    {
    }

    [JsonPropertyName("sn")] public required string SerialNumber { get; set; } = "1";
    [JsonPropertyName("count")] public int Count { get; set; }
    [JsonPropertyName("logindex")] public int PaginationIndex { get; set; }
    [JsonPropertyName("record")] private List<Record> _records { get; set; }

    [JsonIgnore] public List<Record> Records => GetRecords();

    public string Response()
    {
        return "{\"ret\":\"sendlog\",\"count\":" + Count + ",\"logindex\":" + PaginationIndex +
               ",\"result\":true,\"cloudtime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
               "\"}";
    }


    private List<Record> GetRecords()
    {
        foreach (var record in _records) record.DeviceSerialNumber = SerialNumber;

        return _records;
    }
}

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
            var sendLogCommand = JsonSerializer.Deserialize<SendLog>(message) ??
                                 throw new InvalidOperationException();
            _logger.LogInformation(
                $"Received SendLog Command from device {sendLogCommand.SerialNumber} on IP {session.RemoteEndPoint} Session {session.SessionID}");
            var settings = await _settingsProvider.LoadSettings();
            await RecordService.ProcessClockings(sendLogCommand.Records, sendLogCommand.SerialNumber, settings);
            await session.SendAsync(sendLogCommand.Response());
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Error handling SendLog command");
        }
    }
}