using EvoComms.Devices.HanvonVF.Messages.Handlers.Commands.Attributes;
using Microsoft.Extensions.Logging;
using Mina.Core.Session;

namespace EvoComms.Devices.HanvonVF.Messages.Handlers.Commands.UnknownCommand;

public class UnknownCommand : BaseHanvonCommand
{
}

[HanvonCommandHandler("UnknownCommand")]
public class UnknownCommandHandler(ILogger<UnknownCommandHandler> logger) : IHanvonMessageHandler
{
    public Task HandleMessageAsync(IoSession session, object message)
    {
        logger.LogWarning(
            $"Received Command not recognised: {message} from {session.RemoteEndPoint} on session: {session.Id}");
        return Task.CompletedTask;
    }
}