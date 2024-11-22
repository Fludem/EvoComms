using NLog;
using SuperSocket.Connection;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Sessions;

public class TimySocketSession : WebSocketSession
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    public string SerialNumber { get; set; } = "";

    protected override async ValueTask OnSessionConnectedAsync()
    {
        logger.Info("Clocking machine connected.");
        await base.OnSessionConnectedAsync();
    }

    protected override async ValueTask OnSessionClosedAsync(CloseEventArgs e)
    {
        logger.Info("Clocking machine disconnected.");
        await base.OnSessionClosedAsync(e);
    }

    public bool deviceReady()
    {
        return SerialNumber != "";
    }
}