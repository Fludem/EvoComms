using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Messages.Shared;

public abstract class BaseServerResponse : BaseResponse
{
}

public abstract class BaseDeviceResponse : BaseResponse
{
    [JsonPropertyName("sn")] public required string DeviceSerial { get; set; }
}

public abstract class BaseResponse
{
    [JsonPropertyName("result")] public bool Success { get; set; }
    [JsonPropertyName("ret")] public required string Command { get; set; }
}