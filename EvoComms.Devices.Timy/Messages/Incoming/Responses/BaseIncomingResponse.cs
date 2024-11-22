using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Incoming.Responses;

public class BaseIncomingResponse : BaseResponse
{
    [JsonPropertyName("sn")] public required string SerialNumber { get; set; }
}