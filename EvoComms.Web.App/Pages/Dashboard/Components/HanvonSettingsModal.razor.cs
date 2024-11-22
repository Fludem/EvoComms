using EvoComms.Core.Filesystem.Settings;
using EvoComms.Devices.HanvonVF.Settings;
using Microsoft.AspNetCore.Components;

namespace EvoComms.Web.App.Pages.Dashboard.Components;

public partial class HanvonSettingsModal : ComponentBase
{
    private BaseSettingsModal<ModuleSettings> _baseModal;

    private ModuleSettings? _settings;

    [Inject] public required HanvonSettingsProvider HanvonSettingsProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _settings = await HanvonSettingsProvider.LoadSettings();
    }

    public void Show()
    {
        _baseModal.Show();
    }

    private async Task SaveSettings(ModuleSettings settings)
    {
        await HanvonSettingsProvider.SaveSettings(settings);
    }
}