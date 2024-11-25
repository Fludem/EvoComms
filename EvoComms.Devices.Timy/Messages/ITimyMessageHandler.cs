using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages;

public interface ITimyMessageHandler
{
    Task Handle(WebSocketSession session, string message);
}