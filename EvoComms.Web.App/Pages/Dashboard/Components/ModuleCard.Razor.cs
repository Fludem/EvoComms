using Microsoft.AspNetCore.Components;

namespace EvoComms.Web.App.Pages.Dashboard.Components
{
    public partial class ModuleCard
    {
        [Parameter]
        public required string ModuleName { get; set; }

        [Parameter]
        public required string ModuleDescription { get; set; }

        [Parameter]
        public bool IsEnabled { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public EventCallback<bool> OnEnableToggle { get; set; }

        private async Task HandleCardClick()
        {
            await OnClick.InvokeAsync();
        }

        private async Task HandleSwitchClick()
        {
            IsEnabled = !IsEnabled;
            await OnEnableToggle.InvokeAsync(IsEnabled);
        }
    }
}
