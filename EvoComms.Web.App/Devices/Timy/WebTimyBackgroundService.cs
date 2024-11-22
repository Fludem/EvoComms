using EvoComms.Devices.Timy;
using EvoComms.Devices.Timy.Settings;
using SuperSocket.Server.Abstractions;

namespace EvoComms.Web.App.Devices.Timy;

public class WebTimyBackgroundService : BackgroundService
{
    private readonly ILogger<WebTimyBackgroundService> _logger;
    private readonly TimySettingsProvider _settingsProvider;
    private readonly TimyListenerFactory _timyListenerFactory;
    private IServer? _listener;

    public WebTimyBackgroundService(
        ILogger<WebTimyBackgroundService> logger,
        TimySettingsProvider settingsProvider,
        TimyListenerFactory timyListenerFactory)
    {
        _logger = logger;
        _settingsProvider = settingsProvider;
        _timyListenerFactory = timyListenerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
            try
            {
                var settings = await _settingsProvider.LoadSettings();
                if (settings.Enabled && _listener == null)
                {
                    _logger.LogInformation("Starting Timy listener at: {time}", DateTimeOffset.Now);
                    if (await _timyListenerFactory.InitializeServerAsync())
                        _listener = await _timyListenerFactory.StartServerAsync();
                }
                else if (!settings.Enabled && _listener != null)
                {
                    _logger.LogInformation("Stopping Timy listener at: {time}", DateTimeOffset.Now);
                    await _timyListenerFactory.StopServerAsync();
                    _listener = null;
                }

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Timy background service");
            }

        if (_listener != null)
        {
            _logger.LogInformation("Stopping Timy listener at: {time}", DateTimeOffset.Now);
            await _listener.StopAsync();
        }
    }
}