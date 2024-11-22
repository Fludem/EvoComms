using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Events;

public class DeviceRegisteredEventArgs : EventArgs
{
    public DeviceRegisteredEventArgs(string serialNumber, WebSocketSession session)
    {
        this.serialNumber = serialNumber;
        this.session = session;
    }

    public string serialNumber { get; }
    public WebSocketSession session { get; }
}