using EvoComms.Devices.HanvonVF.HanvonServer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EvoComms.Devices.HanvonVF.BackgroundServices;

public class HanvonService(
    HanvonListenerManager hanvonListenerManager,
    ILogger<HanvonService> logger)
    : BackgroundService
{
    private readonly ILogger<HanvonService> _logger = logger;
    private HanvonListener? _listener;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Hanvon Service Starting");
            // 9922 is default port on most Hanvon Devices
            _listener = hanvonListenerManager.CreateListener(9922);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}