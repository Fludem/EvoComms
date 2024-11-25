using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Logging;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.Shared;

public abstract class BaseTimyMessageHandler(
    ILogger logger,
    RecordService recordService,
    TimySettingsProvider timySettingsManager)
    : ITimyMessageHandler
{
    protected readonly ILogger Logger = logger;
    protected readonly RecordService RecordService = recordService;
    protected readonly TimySettingsProvider SettingsProvider = timySettingsManager;

    public abstract Task Handle(WebSocketSession session, string message);
}