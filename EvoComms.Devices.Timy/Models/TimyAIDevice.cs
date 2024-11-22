namespace EvoComms.Devices.Timy.Models;

public class TimyAIDevice
{
    private DeviceInfo _deviceInfo;

    public TimyAIDevice(DeviceInfo deviceInfo)
    {
        _deviceInfo = deviceInfo;
    }

    public static TimyAIDevice Make(DeviceInfo deviceInfo)
    {
        return new TimyAIDevice(deviceInfo);
    }

    public void DownloadClockings(bool onlyNewRecords = true)
    {
    }
}