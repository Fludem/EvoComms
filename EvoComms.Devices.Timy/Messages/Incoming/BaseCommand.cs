using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Incoming;

public class BaseCommand
{
    [JsonPropertyName("ret")] public required string Command { get; set; }

    protected string GetTimeString()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}