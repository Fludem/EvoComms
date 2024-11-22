using EvoComms.Core.Filesystem.Settings.Providers;

namespace EvoComms.Devices.ZKTeco.Settings.Interfaces;

public interface IZkSettingsProvider : IModuleSettingsProvider
{
    int GetTimezone();
}