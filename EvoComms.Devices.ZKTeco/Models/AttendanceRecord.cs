namespace EvoComms.Devices.ZKTeco.Models;

public class AttendanceRecord
{
    public string? UserId { get; set; }
    public DateTime? DateTime { get; set; }
    public string? InOutMode { get; set; }
    public string? VerifyMode { get; set; }
    public string? EventCode { get; set; }
    public string? Mask { get; set; }
    public string? Temperature { get; set; }
}