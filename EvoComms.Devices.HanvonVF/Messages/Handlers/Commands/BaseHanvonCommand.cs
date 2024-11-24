using Newtonsoft.Json;

namespace EvoComms.Devices.HanvonVF.Messages.Handlers.Commands;

public class BaseHanvonCommand
{
    [JsonProperty("COMMAND")] public required string Command { get; set; }
}