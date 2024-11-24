using Mina.Core.Session;

namespace EvoComms.Devices.HanvonVF.Messages.Handlers;

public interface IHanvonMessageHandler
{
    Task HandleMessageAsync(IoSession session, object message);
}