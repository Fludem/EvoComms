using System.Text.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SuperSocket.WebSocket.Server;

namespace EvoComms.Devices.Timy.Messages;

public class TimyMessageProcessor
{
    private readonly TimyHandlerRegistry _handlerRegistry;
    private readonly ILogger<TimyMessageProcessor> _logger;

    public TimyMessageProcessor(
        ILogger<TimyMessageProcessor> logger,
        TimyHandlerRegistry handlerRegistry)
    {
        _logger = logger;
        _handlerRegistry = handlerRegistry;
    }

    public async Task ProcessMessageAsync(WebSocketSession session, string message)
    {
        try
        {
            _logger.LogDebug("Processing message from session {SessionId}: {Message}",
                session.SessionID, message);

            var jsonMsg = JObject.Parse(message);
            var cmdType = jsonMsg.Value<string>("cmd");
            var responseType = jsonMsg.Value<string>("ret");

            if (!string.IsNullOrEmpty(cmdType))
                await HandleCommandAsync(cmdType, message, session);
            else if (!string.IsNullOrEmpty(responseType))
                await HandleResponseAsync(responseType, message, session);
            else
                _logger.LogWarning("Unrecognized message format from session {SessionId}", session.SessionID);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing message from session {session.SessionID}: {message}");
            await HandleErrorAsync(session, ex);
        }
    }

    private async Task HandleCommandAsync(string cmdType, string message, WebSocketSession session)
    {
        var handler = _handlerRegistry.GetCommandHandler(cmdType);
        if (handler != null)
            await handler.Handle(session, message);
        else
            _logger.LogWarning("No handler found for command {Command}", cmdType);
    }

    private async Task HandleResponseAsync(string responseType, string message, WebSocketSession session)
    {
        var handler = _handlerRegistry.GetResponseHandler(responseType);
        if (handler != null)
            await handler.Handle(session, message);
        else
            _logger.LogWarning("No handler found for response {ResponseType}", responseType);
    }

    private async Task HandleErrorAsync(WebSocketSession session, Exception ex)
    {
        var errorMessage = new { error = "Error processing message", details = ex.Message };
        await session.SendAsync(JsonSerializer.Serialize(errorMessage));
    }
}