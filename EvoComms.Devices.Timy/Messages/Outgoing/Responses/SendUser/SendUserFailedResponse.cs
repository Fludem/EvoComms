using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Outgoing.Responses.SendUser;

public class SendUserFailResponse : SendUserResponse
{
    public SendUserFailResponse(int reason) : base(false)
    {
        Reason = reason;
    }

    [JsonPropertyName("reason")] public int Reason { get; set; }
}