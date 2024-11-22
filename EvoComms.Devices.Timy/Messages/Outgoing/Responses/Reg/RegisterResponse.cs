using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Outgoing.Responses.Reg;

public class RegisterResponse : BaseResponse
{
    private RegisterResponse(string ret, bool result, string? cloudtime = null)
    {
        Command = ret;
        Success = result;
        this.cloudtime = cloudtime ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public RegisterResponse(bool success)
        : this("reg", success)
    {
    }

    public string cloudtime { get; set; }
    [JsonPropertyName("nosenduser")] public bool DisabledNewUserSending { get; set; } = true;
}