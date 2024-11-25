using EvoComms.Devices.HanvonVF.Messages.Handlers.Commands.Attributes;
using Microsoft.Extensions.Logging;
using Mina.Core.Session;
using Newtonsoft.Json;

namespace EvoComms.Devices.HanvonVF.Messages.Handlers.Commands.GetRequest;

public class GetRequestCommand : BaseHanvonCommand
{
    [JsonProperty("PARAM")] public required GetRequestParams Parameters { get; set; }
}

[HanvonCommandHandler("GetRequest")]
public class GetRequestHandler : IHanvonMessageHandler
{
    private readonly ILogger<GetRequestHandler> _logger;

    public GetRequestHandler(ILogger<GetRequestHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleMessageAsync(IoSession session, object message)
    {
        _logger.LogInformation("Handling GetRequest");
        var command = message as GetRequestCommand;
        if (command != null)
            // Handle the GetRequest command
            _logger.LogInformation($"Handling GetRequest for SN: {command.Parameters.SerialNumber}");
        return Task.CompletedTask;
    }
}

public class GetRequestParams
{
    [JsonProperty("SN")] public required string SerialNumber { get; set; }
}