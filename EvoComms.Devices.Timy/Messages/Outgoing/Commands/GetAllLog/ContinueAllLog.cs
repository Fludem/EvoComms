using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Outgoing.Commands.GetAllLog;

public class ContinueAllLog
{
    [JsonPropertyName("cmd")] public string cmd { get; set; } = "getalllog";
    [JsonPropertyName("stn")] public bool stn { get; set; }
}