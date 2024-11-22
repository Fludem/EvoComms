using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Outgoing.Commands.GetAllLog;

public class GetAllLogCommand
{
    [JsonPropertyName("cmd")] public string cmd { get; set; } = "getalllog";
    [JsonPropertyName("stn")] public bool stn { get; set; } = true;
    [JsonPropertyName("from")] public string from { get; set; } = "2024-06-20";
    [JsonPropertyName("to")] public string to { get; set; } = "2024-10-25";
}