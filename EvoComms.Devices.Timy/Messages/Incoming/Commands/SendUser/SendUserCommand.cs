using System.Text.Json.Serialization;
using EvoComms.Devices.Timy.Messages.Incoming.Commands.Interfaces;
using EvoComms.Devices.Timy.Messages.Outgoing.Responses.SendUser;
using Newtonsoft.Json;

namespace EvoComms.Devices.Timy.Messages.Incoming.Commands.SendUser;

public class SendUserCommand : IIncomingCommand
{
    [JsonPropertyName("cmd")] public string cmd { get; set; }

    [JsonPropertyName("enrollid")] public int EnrollId { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

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