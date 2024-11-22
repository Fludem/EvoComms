using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EvoComms.Web.App.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitPreline();
            }
        }

        /// <summary>
        /// Initializes the Preline plugin.
        /// This method is called after the component has rendered for the first time,
        /// ensuring that the plugin is initialized when required.
        /// It uses JavaScript interop to invoke the 'initPreline' function in the browser.
        /// </summary>
        private async Task InitPreline()
        {
            await JSRuntime.InvokeVoidAsync("initPreline");
        }

        [JSInvokable]
        public async Task OnLocationChanged()
        {
            await JSRuntime.InvokeVoidAsync("initPreline");
        }
    }
}

