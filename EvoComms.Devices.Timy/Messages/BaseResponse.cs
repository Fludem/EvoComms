using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages;

public class BaseResponse : IResponseMessage
{
    [JsonPropertyName("result")] public bool Success { get; set; }
    [JsonPropertyName("ret")] public string Command { get; set; }

    private string GetTimeString()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}