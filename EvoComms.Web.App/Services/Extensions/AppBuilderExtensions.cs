using System.ServiceProcess;
using NLog;

namespace EvoComms.Web.App.Services.Extensions;

public static class AppBuilderExtensions
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static void ConfigureApp(this WebApplication app, bool isService)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        else
        {
            DoServiceChecks(app, isService);
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
    }

    public static void DoServiceChecks(this WebApplication app, bool isService)
    {
        if (!isService)
            try
            {
                using (var serviceController = new ServiceController("EvoCommsService"))
                {
                    if (serviceController.Status == ServiceControllerStatus.Running)
                        Logger.Error(
                            "EvoComms is already running as a service. Please first stop the service before running Debug Console.");
                }
            }
            catch (InvalidOperationException)
            {
                Logger.Info("The EvoCommsService does not exist. Continuing in console mode.");
            }
    }
}