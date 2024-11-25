using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using EvoComms.Devices.Timy.Models;

namespace EvoComms.Devices.Timy.Messages.Incoming.Commands.Reg;

public class RegCommand : BaseCommand
{
    [JsonPropertyName("devinfo")] public required DeviceInfo DeviceInfo { get; set; }
    [JsonPropertyName("sn")] public required string SerialNumber { get; set; } = "1";

    public string Response()
    {
        var response = JsonSerializer.Serialize(new RegisterResponse(true));
        return response;
    }
}

public class RegisterResponse : BaseResponse
{
    [SetsRequiredMembers]
    public RegisterResponse(bool success, string? serverTime = null)
    {
        Command = "reg";
        Success = success;
        ServerTime = serverTime ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    [JsonPropertyName("cloudtime")] public required string ServerTime { get; set; }
    [JsonPropertyName("nosenduser")] public bool DisabledNewUserSending { get; set; } = true;
}