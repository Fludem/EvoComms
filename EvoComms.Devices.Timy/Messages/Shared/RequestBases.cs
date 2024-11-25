using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Shared;

public abstract class BaseRequest
{
    [JsonPropertyName("cmd")] public required string Command { get; set; }
}

public abstract class BaseDeviceRequest : BaseRequest
{
    [JsonPropertyName("sn")] public required string DeviceSerial { get; set; }
}

public abstract class BaseServerRequest : BaseRequest
{
}