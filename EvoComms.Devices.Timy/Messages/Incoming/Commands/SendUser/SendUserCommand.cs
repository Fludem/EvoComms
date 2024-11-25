using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace EvoComms.Devices.Timy.Messages.Incoming.Commands.SendUser;

public class SendUserCommand : BaseCommand
{
    [JsonPropertyName("enrollid")] public int EnrollId { get; set; }

    [JsonPropertyName("name")] public required string Name { get; set; } = "NA";

    [JsonPropertyName("backupnum")] public int BackupNum { get; set; }

    [JsonPropertyName("admin")] public int Admin { get; set; }


    /// <remarks>
    ///     TODO: Replace with proper typed model for polymorphic record objects.
    ///     Will be refactored to use proper inheritance/polymorphic model to handle
    ///     different Employee types. It takes sometimes at the object is structured
    ///     differently if they are fingerprint, or face, or card etc.
    /// </remarks>
    [JsonPropertyName("record")]
    public string? Employees { get; set; }

    public string Response()
    {
        var response = JsonConvert.SerializeObject(new SendUserResponse(true));
        return response;
    }
}

public class SendUserResponse : BaseResponse
{
    [SetsRequiredMembers]
    public SendUserResponse(bool result)
    {
        Command = "senduser";
        Result = result;
        CloudTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    [JsonPropertyName("result")] public bool Result { get; set; }

    [JsonPropertyName("cloudtime")] public string CloudTime { get; set; }
}

[method: SetsRequiredMembers]
public class SendUserFailResponse(int reason) : SendUserResponse(false)
{
    [JsonPropertyName("reason")] public int Reason { get; set; } = reason;
}