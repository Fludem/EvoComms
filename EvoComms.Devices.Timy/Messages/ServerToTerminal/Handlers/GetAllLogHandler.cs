using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Messages.Attributes;
using EvoComms.Devices.Timy.Messages.Shared;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.ServerToTerminal.Handlers;

[TimyResponseHandler("getalllog")]
public class GetAllLogHandler(
    ILogger<GetAllLogHandler> logger,
    RecordService recordService,
    TimySettingsProvider timySettingsProvider)
    : BaseTimyMessageHandler(logger, recordService, timySettingsProvider)
{
    public override async Task Handle(WebSocketSession session, string message)
    {
        var getAllLogResponse = JsonConvert.DeserializeObject<GetAllLogResponse>(message) ??
                                throw new InvalidOperationException();
        if (!AllClockingsCollected(getAllLogResponse))
            await session.SendAsync(JsonConvert.SerializeObject(new ContinueAllLog()));

        var settings = await SettingsProvider.LoadSettings();
        await RecordService.ProcessClockings(getAllLogResponse.GetRecords(), getAllLogResponse.DeviceSerial,
            settings);
    }

    private bool AllClockingsCollected(GetAllLogResponse getAllLogResponse)
    {
        if (getAllLogResponse.TotalClockingsCount != getAllLogResponse.EndIndex)
        {
            var clockingsRemainingCount = getAllLogResponse.TotalClockingsCount - getAllLogResponse.EndIndex;
            Logger.LogInformation(
                $"Device {getAllLogResponse.DeviceSerial} still has {clockingsRemainingCount} left to collect. Received {getAllLogResponse.EndIndex} out of {getAllLogResponse.TotalClockingsCount}");
            return false;
        }

        Logger.LogInformation(
            $"Device {getAllLogResponse.DeviceSerial} has finished sending all clockings. Total Clockings: {getAllLogResponse.TotalClockingsCount}");
        return true;
    }
}