using EvoComms.Core.Filesystem.Settings;
using EvoComms.Core.Filesystem.Settings.Providers;
using Microsoft.Extensions.Logging;

namespace EvoComms.Devices.Timy.Settings;

public class TimySettingsProvider : ModuleSettingsProvider<ModuleSettings>
{
    public TimySettingsProvider(ILogger<TimySettingsProvider> logger)
        : base("TimySettings.json", logger)
    {
    }

    protected override ModuleSettings CreateDefaultSettings()
    {
        return new ModuleSettings
        {
            ListenPort = 7788,
            Enabled = false,
            OutputPath = "C:/Info"
        };
    }
}