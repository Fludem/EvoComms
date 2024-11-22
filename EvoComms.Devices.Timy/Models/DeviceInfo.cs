using System.Text.Json.Serialization;

namespace EvoComms.Devices.Timy.Models;

public class DeviceInfo
{
    [JsonPropertyName("modelname")] public string DeviceModel { get; set; } = "";
    [JsonPropertyName("usersize")] public int TotalCapacity { get; set; }
    [JsonPropertyName("facesize")] public int FaceCapacity { get; set; }
    [JsonPropertyName("fpsize")] public int FingerprintsCapacity { get; set; }
    [JsonPropertyName("cardsize")] public int CardCapacity { get; set; }
    [JsonPropertyName("pwdsize")] public int PasswordCapacity { get; set; }
    [JsonPropertyName("logsize")] public int ClockingCapacity { get; set; }
    [JsonPropertyName("useduser")] public int EnrolledUserCount { get; set; }
    [JsonPropertyName("usedface")] public int FaceEnrolledCount { get; set; }
    [JsonPropertyName("usedfp")] public int FingerprintsEnrolledCount { get; set; }
    [JsonPropertyName("usedcard")] public int CardEnrollmentCount { get; set; }
    [JsonPropertyName("usedpwd")] public int PasswordsEnrolledCount { get; set; }
    [JsonPropertyName("usedlog")] public int NewClockingCount { get; set; }
    [JsonPropertyName("usednewlog")] public int TotalClockingCount { get; set; }
    [JsonPropertyName("usedrtlog")] public int RealTimeLogCount { get; set; }
    [JsonPropertyName("netinuse")] public int IsNetworkEnabled { get; set; }
    [JsonPropertyName("usb4g")] public int IsDongleEnabled { get; set; }
    [JsonPropertyName("fpalgo")] public string FingerprintAlgorithmVersion { get; set; } = "Unknown";
    [JsonPropertyName("firmware")] public string FirmwareVersion { get; set; } = "Unknown";
    [JsonPropertyName("time")] public string DeviceTime { get; set; } = "Unknown";
    [JsonPropertyName("intercom")] public int IntercomEnabled { get; set; }
    [JsonPropertyName("floors")] public int TotalFloors { get; set; }
    [JsonPropertyName("charid")] public int CharacterId { get; set; }

    /// <summary>
    ///     indicates whether the Open Supervised Device Protocol (OSDP) is enabled.
    ///     OSDP is a communication standard for connecting access control and security devices.
    ///     It ensures secure communication and interoperability between various types of access control equipment.
    /// </summary>
    [JsonPropertyName("useosdp")]
    public int OsdpEnabled { get; set; }

    [JsonPropertyName("dislanguage")] public int DisplayLanguage { get; set; }
    [JsonPropertyName("mac")] public string MacAddress { get; set; } = "Unknown";
}