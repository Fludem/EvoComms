using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Incoming.Responses;

public class BaseServerIncomingResponse : BaseServerResponse
{
    [JsonPropertyName("sn")] public required string SerialNumber { get; set; }
}