using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages;

public interface IMessageHandler
{
    Task Handle(WebSocketSession session, string message);
}