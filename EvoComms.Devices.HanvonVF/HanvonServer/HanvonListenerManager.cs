using EvoComms.Devices.HanvonVF.Messages.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mina.Transport.Socket;

namespace EvoComms.Devices.HanvonVF.HanvonServer;

public class HanvonListenerManager(ILogger<HanvonListenerManager> logger, IServiceProvider serviceProvider)
{
    private readonly Dictionary<int, HanvonListener> _activeListeners = new();

    public HanvonListener CreateListener(int port)
    {
        if (_activeListeners.ContainsKey(port))
            throw new InvalidOperationException($"A listener on port {port} already exists");

        logger.LogInformation($"Creating Hanvon Listener on port {port}");
        using var scope = serviceProvider.CreateScope();
        var hanvonLogger = scope.ServiceProvider.GetRequiredService<ILogger<HanvonListener>>();
        var hanvonHandlerRegistry = scope.ServiceProvider.GetRequiredService<HanvonHandlerRegistry>();
        var tcpServer = new AsyncSocketAcceptor();

        var listener = new HanvonListener(port, tcpServer, hanvonLogger, hanvonHandlerRegistry);
        listener.Start();
        logger.LogInformation($"Hanvon Listener on port {port} started");
        _activeListeners[port] = listener;
        return listener;
    }

    public async Task RemoveListener(int port)
    {
        if (_activeListeners.TryGetValue(port, out var listener))
        {
            await listener.StopAsync();
            _activeListeners.Remove(port);
        }
    }
}