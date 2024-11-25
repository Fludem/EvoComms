using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Incoming;

public class BaseResponse
{
    [JsonPropertyName("result")] public bool Success { get; set; }
    [JsonPropertyName("ret")] public required string Command { get; set; }

    protected string GetTimeString()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}