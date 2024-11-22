using EvoComms.Devices.ZKTeco.Settings;
using Microsoft.AspNetCore.Components;

namespace EvoComms.Web.App.Pages.Dashboard.Components;

public partial class ZkSettingsModal : ComponentBase
{
    public required BaseSettingsModal<ZkModuleSettings> BaseModal;

    public required ZkModuleSettings Settings;

    [Inject] public required ZkSettingsProvider ZkSettingsProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Settings = await ZkSettingsProvider.LoadSettings();
    }

    public void Show()
    {
        BaseModal.Show();
    }

    public async Task SaveSettings(ZkModuleSettings moduleSettings)
    {
        await ZkSettingsProvider.SaveSettings(moduleSettings);
    }
}