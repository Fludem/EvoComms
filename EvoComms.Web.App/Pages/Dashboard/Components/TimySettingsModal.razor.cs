using System.Text;
using EvoComms.Core.Filesystem.Settings;
using EvoComms.Core.Models;
using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EvoComms.Web.App.Pages.Dashboard.Components;

public partial class TimySettingsModal : ComponentBase
{
    private BaseSettingsModal<ModuleSettings>? _baseModal;

    private ModuleSettings? _settings;

    [Inject] public required TimySettingsProvider TimySettingsProvider { get; set; }
    [Inject] public required RecordService RecordService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _settings = await TimySettingsProvider.LoadSettings();
    }

    public void Show()
    {
        _baseModal?.Show();
    }

    private async Task SaveSettings(ModuleSettings settings)
    {
        await TimySettingsProvider.SaveSettings(settings);
    }

    private async Task UploadFile(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file.ContentType != "text/plain") throw new InvalidOperationException("Please upload a text file.");
        using var stream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(stream);

        var content = Encoding.UTF8.GetString(stream.ToArray());
        var records = await Record.FromTimyUsbFile(content);

        var settings = await TimySettingsProvider.LoadSettings();
        await RecordService.ProcessClockings(records, "USB", settings);
    }
}