using EvoComms.Core.Filesystem.Settings;
using EvoComms.Core.Filesystem.Settings.Providers;
using Microsoft.Extensions.Logging;

namespace EvoComms.Devices.HanvonVF.Settings;

public class HanvonSettingsProvider : ModuleSettingsProvider<ModuleSettings>
{
    public HanvonSettingsProvider(ILogger<HanvonSettingsProvider> logger)
        : base("HanvonSettings.json", logger)
    {
    }

    protected override ModuleSettings CreateDefaultSettings()
    {
        return new ModuleSettings
        {
            ListenPort = 9922,
            Enabled = false,
            OutputPath = "C:/Info"
        };
    }
}