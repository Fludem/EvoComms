using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Logging;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.Incoming;

public abstract class BaseITimyMessageHandler : ITimyMessageHandler
{
    protected readonly ILogger _logger;
    protected readonly TimySettingsProvider _settingsProvider;
    protected readonly RecordService RecordService;

    protected BaseITimyMessageHandler(
        ILogger logger,
        RecordService recordService,
        TimySettingsProvider timySettingsManager)
    {
        _logger = logger;
        RecordService = recordService;
        _settingsProvider = timySettingsManager;
    }

    public abstract Task Handle(WebSocketSession session, string message);
}