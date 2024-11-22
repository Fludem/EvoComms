using EvoComms.Core.Filesystem.Settings;
using Microsoft.AspNetCore.Components;

namespace EvoComms.Web.App.Pages.Dashboard.Components;

public partial class BaseSettingsModal<TSettings> : ComponentBase where TSettings : ModuleSettings
{
    private bool _isRendered;

    private bool _isVisible;
    [Parameter] public string Title { get; set; } = "";
    [Parameter] public string Description { get; set; } = "";
    [Parameter] public TSettings? Settings { get; set; }
    [Parameter] public EventCallback<TSettings> OnSave { get; set; }
    [Parameter] public required RenderFragment AdditionalContent { get; set; }

    public void Show()
    {
        _isRendered = true;
        _isVisible = true;
        StateHasChanged();
    }

    private void Close()
    {
        _isVisible = false;
        StateHasChanged();
        // Use a timer to remove the element from the DOM after the animation
        var timer = new Timer(_ =>
        {
            _isRendered = false;
            InvokeAsync(StateHasChanged);
        }, null, 300, Timeout.Infinite); // 300ms matches the motion-duration-300 class
    }

    public async Task SaveSettings()
    {
        await OnSave.InvokeAsync(Settings);
        Close();
    }

    public void ToggleEnabled()
    {
        Settings.Enabled = !Settings.Enabled;
    }

    public void CloseOnOutsideClick()
    {
        if (_isVisible) Close();
    }

    public async Task CloseModal()
    {
        _isVisible = false;
        StateHasChanged();
        await Task.Delay(300); // Delay to match the animation duration
        _isRendered = false;
        StateHasChanged();
    }
}