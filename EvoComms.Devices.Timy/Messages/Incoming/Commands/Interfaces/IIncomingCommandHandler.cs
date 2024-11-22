using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages.Incoming.Commands.Interfaces;

public interface IIncomingCommandHandler
{
    Task Handle(WebSocketSession session, string message);
}