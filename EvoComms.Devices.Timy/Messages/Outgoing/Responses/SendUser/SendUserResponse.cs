using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Outgoing.Responses.SendUser;

public class SendUserResponse
{
    public SendUserResponse(bool result)
    {
        Result = result;
        CloudTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    [JsonPropertyName("ret")] public string ReturnCommand { get; set; } = "senduser";

    [JsonPropertyName("result")] public bool Result { get; set; }

    [JsonPropertyName("cloudtime")] public string CloudTime { get; set; }
}