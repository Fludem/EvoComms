using System.Reflection;
using EvoComms.Devices.Timy.Messages.Attributes;
using Microsoft.Extensions.Logging;

namespace EvoComms.Devices.Timy.Messages;

public class TimyHandlerRegistry
{
    private readonly Dictionary<string, ITimyMessageHandler> _commandHandlers;
    private readonly ILogger<TimyHandlerRegistry> _logger;
    private readonly Dictionary<string, ITimyMessageHandler> _responseHandlers;

    public TimyHandlerRegistry(
        ILogger<TimyHandlerRegistry> logger,
        IEnumerable<ITimyMessageHandler> handlers)
    {
        _logger = logger;
        _commandHandlers = new Dictionary<string, ITimyMessageHandler>();
        _responseHandlers = new Dictionary<string, ITimyMessageHandler>();

        RegisterHandlers(handlers);
    }

    private void RegisterHandlers(IEnumerable<ITimyMessageHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            var handlerType = handler.GetType();
            var commandAttr = handlerType.GetCustomAttribute<TimyCommandHandlerAttribute>();
            var responseAttr = handlerType.GetCustomAttribute<TimyResponseHandlerAttribute>();

            if (commandAttr != null)
            {
                _commandHandlers[commandAttr.Command] = handler;
                _logger.LogInformation("Registered command handler for {Command}", commandAttr.Command);
            }
            else if (responseAttr != null)
            {
                _responseHandlers[responseAttr.ResponseType] = handler;
                _logger.LogInformation("Registered response handler for {ResponseType}", responseAttr.ResponseType);
            }
        }
    }

    public ITimyMessageHandler? GetCommandHandler(string command)
    {
        _commandHandlers.TryGetValue(command, out var handler);
        return handler;
    }

    public ITimyMessageHandler? GetResponseHandler(string responseType)
    {
        _responseHandlers.TryGetValue(responseType, out var handler);
        return handler;
    }
}