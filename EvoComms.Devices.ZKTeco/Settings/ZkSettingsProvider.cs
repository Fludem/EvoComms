using EvoComms.Core.Filesystem.Settings.Providers;
using EvoComms.Devices.ZKTeco.Settings.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoComms.Devices.ZKTeco.Settings;

public class ZkSettingsProvider : ModuleSettingsProvider<ZkModuleSettings>, IZkSettingsProvider
{
    public ZkSettingsProvider(ILogger<ZkSettingsProvider> logger)
        : base("ZKSettings.json", logger)
    {
    }

    public int GetTimezone()
    {
        return _settings.Timezone;
    }

    protected override ZkModuleSettings CreateDefaultSettings()
    {
        return new ZkModuleSettings
        {
            ListenPort = 17856,
            Enabled = false,
            OutputPath = "C:/Info",
            Timezone = 0
        };
    }
}