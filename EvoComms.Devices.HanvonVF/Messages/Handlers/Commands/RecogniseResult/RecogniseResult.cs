using EvoComms.Devices.HanvonVF.Messages.Handlers.Commands.Attributes;
using Microsoft.Extensions.Logging;
using Mina.Core.Session;
using Newtonsoft.Json;

namespace EvoComms.Devices.HanvonVF.Messages.Handlers.Commands.RecogniseResult;

[HanvonCommandHandler("RecogniseResult")]
public class RecogniseResultHandler : IHanvonMessageHandler
{
    private readonly ILogger<RecogniseResultHandler> _logger;

    public RecogniseResultHandler(ILogger<RecogniseResultHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleMessageAsync(IoSession session, object message)
    {
        _logger.LogInformation("Handling RecogniseResult");
        var response = message as RecogniseResultCommand;
        if (response != null)
            _logger.LogInformation($"Handling RecogniseResult for ID: {response.Parameters.EmployeeId}");
        return Task.CompletedTask;
    }
}

public class RecogniseResultParams
{
    [JsonProperty("SN")] public required string SerialNumber { get; set; }
    [JsonProperty("ID")] public required string EmployeeId { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    [JsonProperty("AlgEdition")] public required string AlgVersion { get; set; } = "NA";
    public bool IsUserUpdate { get; set; } = true;
    public required string CaptureJpg { get; set; } = "NA";
    public double Score { get; set; }
    public int Pass { get; set; }
    public double Threshold { get; set; }
    [JsonProperty("Time")] public required string ClockedAt { get; set; }
    public required string RecogType { get; set; }
    public required string IdentifyType { get; set; }
    public double AnimalHeat { get; set; }
    public int WearMask { get; set; }
}

public class RecogniseResultCommand(RecogniseResultParams parameters) : BaseHanvonCommand
{
    [JsonProperty("PARAM")] public RecogniseResultParams Parameters { get; set; } = parameters;
}