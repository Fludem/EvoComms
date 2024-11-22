using System.Text.Json;
using System.Text.Json.Serialization;
using EvoComms.Devices.Timy.Messages.Incoming.Commands.Interfaces;
using EvoComms.Devices.Timy.Messages.Outgoing.Responses.Reg;
using EvoComms.Devices.Timy.Models;

namespace EvoComms.Devices.Timy.Messages.Incoming.Commands.Reg;

public class RegCommand : IIncomingCommand
{
    [JsonPropertyName("devinfo")] public DeviceInfo deviceInfo { get; set; }
    [JsonPropertyName("cmd")] public string cmd { get; set; }
    [JsonPropertyName("sn")] public string sn { get; set; }

    public string Response()
    {
        var response = JsonSerializer.Serialize(new RegisterResponse(true));
        return response;
    }
}