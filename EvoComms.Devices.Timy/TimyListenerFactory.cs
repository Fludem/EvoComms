using EvoComms.Devices.Timy.Messages;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy;

public class TimyListenerFactory : IAsyncDisposable
{
    private readonly ILogger<TimyListenerFactory> _logger;
    private readonly TimyMessageProcessor _messageProcessor;
    private readonly TimySettingsProvider _timySettingsProvider;
    private IServer? _webSocketServer;

    public TimyListenerFactory(
        ILogger<TimyListenerFactory> logger, TimySettingsProvider timySettingsProvider,
        TimyMessageProcessor messageProcessor)
    {
        _logger = logger;
        _timySettingsProvider = timySettingsProvider;
        _messageProcessor = messageProcessor;
        _logger.LogDebug("TimyListenerFactory Instantiated.");
    }

    public async ValueTask DisposeAsync()
    {
        if (_webSocketServer != null)
        {
            await StopServerAsync();
            if (_webSocketServer is IAsyncDisposable disposable) await disposable.DisposeAsync();

            _webSocketServer = null;
        }
    }

    public async Task<bool> InitializeServerAsync()
    {
        _logger.LogInformation("Initializing WebSocket Server.");
        try
        {
            var timySettings = await _timySettingsProvider.LoadSettings();

            var builder = WebSocketHostBuilder.Create();

            _logger.LogInformation(
                $"Timy Socket Server is being created. Output Path: {timySettings.OutputPath} Type: {timySettings.OutputType.ToString()}");

            builder.UseWebSocketMessageHandler(
                async (session, message) =>
                {
                    _logger.LogDebug($"Received message: {message.Message}");
                    await _messageProcessor.ProcessMessageAsync((WebSocketSession)session, message.Message);
                }
            );

            builder.ConfigureAppConfiguration((hostCtx, configApp) =>
            {
                configApp.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "serverOptions:name", "TimyServer" },
                    { "serverOptions:listeners:0:ip", "0.0.0.0" },
                    { "serverOptions:listeners:0:port", "7788" }
                });
            });

            _webSocketServer = builder.BuildAsServer();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to initialize WebSocket server due to error.");
            return false;
        }
    }

    public async Task<IServer?> StartServerAsync()
    {
        if (_webSocketServer != null)
        {
            await _webSocketServer.StartAsync();
            _logger.LogInformation("WebSocket server started successfully.");
            return _webSocketServer;
        }

        _logger.LogError("WebSocket server is not initialized. Call InitializeServerAsync first.");
        return null;
    }

    public async Task StopServerAsync()
    {
        if (_webSocketServer != null && _webSocketServer.State == ServerState.Started)
        {
            await _webSocketServer.StopAsync();
            _logger.LogInformation("WebSocket server stopped successfully.");
        }
    }
}