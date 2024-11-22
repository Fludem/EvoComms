using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages;

public interface IResponseMessage
{
    [JsonPropertyName("ret")] string Command { get; set; }

    [JsonPropertyName("result")] bool Success { get; set; }
}