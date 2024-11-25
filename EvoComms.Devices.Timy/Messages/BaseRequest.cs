using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages;

public class BaseDeviceRequest
{
    [JsonPropertyName("cmd")] public required string Command { get; set; }
}

public class BaseServerRequest
{
    [JsonPropertyName("cmd")] public required string Command { get; set; }

    protected string GetTimeString()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}