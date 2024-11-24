using System.Reflection;
using Com.FirstSolver.Splash;
using EvoComms.Devices.HanvonVF.Messages.Handlers.Commands.Attributes;
using Microsoft.Extensions.Logging;
using Mina.Handler.Demux;

namespace EvoComms.Devices.HanvonVF.Messages.Handlers;

public class HanvonHandlerRegistry
{
    private readonly Dictionary<string, IMessageHandler> _commandHandlers = new();
    private readonly ILogger<HanvonHandlerRegistry> _logger;

    public HanvonHandlerRegistry(ILogger<HanvonHandlerRegistry> logger, IEnumerable<IMessageHandler> handlers)
    {
        _logger = logger;
        RegisterHandlers(handlers);
    }

    private void RegisterHandlers(IEnumerable<IMessageHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            var handlerType = handler.GetType();
            var commandAttr = handlerType.GetCustomAttribute<HanvonCommandHandlerAttribute>();

            if (commandAttr == null) continue;
            _commandHandlers[commandAttr.Command] = handler;
            _logger.LogInformation("Registered command handler for {Command}", commandAttr.Command);
        }
    }

    public IMessageHandler? GetHandler(string incomingMessage)
    {
        var incomingMessageObject = incomingMessage.FromJsonTo<COMMAND_PCIR_TYPE>();
        return _commandHandlers.GetValueOrDefault(incomingMessageObject.COMMAND);
    }
}