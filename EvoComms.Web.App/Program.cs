using System.Diagnostics;
using System.Net;
using System.ServiceProcess;
using EvoComms.Core.Database;
using EvoComms.Core.Services.Extensions;
using EvoComms.Devices.HanvonVF;
using EvoComms.Devices.HanvonVF.BackgroundServices;
using EvoComms.Devices.Timy;
using EvoComms.Devices.ZKTeco.Http.Controllers;
using EvoComms.Devices.ZKTeco.Services;
using EvoComms.Logging.Services;
using EvoComms.Web.App.Devices.Timy;
using EvoComms.Web.App.TLS;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var programDataPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
    "ClockingSystems",
    "EvoComms"
);

if (args.Contains("--install-cert"))
{
    HttpsHelper.SetupHttps(programDataPath);
    return;
}

try
{
    // Checks if running as service or console
    var isService = !Environment.UserInteractive;

    // Creates the Logs folder if it doesn't exist and stores it in the variable for NLOG.
    var logsPath = Path.Combine(programDataPath, "Logs");
    Directory.CreateDirectory(logsPath);

    var builder = WebApplication.CreateBuilder(args);

    var certificate = HttpsHelper.EnsureHttpsCertificate(programDataPath);
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        // HTTP to HTTPS redirect
        serverOptions.Listen(IPAddress.Any, 80, listenOptions => { listenOptions.UseHttps(certificate); });

        // Main HTTPS endpoint
        serverOptions.Listen(IPAddress.Any, 443, listenOptions => { listenOptions.UseHttps(certificate); });
    });

    if (isService)
    {
        var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
        var pathToContentRoot = Path.GetDirectoryName(pathToExe);
        builder.Host.UseContentRoot(pathToContentRoot!);
        builder.Host.UseWindowsService(options => { options.ServiceName = "EvoCommsService"; });
    }

    // Adding DB Context which essentially is how we interact with the DB
    builder.Services.AddDbContext<AppDbContext>();

    // Adding Clock Specific Services and File Writers to Containers
    builder.Services.AddTimyServices().AddZkServices().AddHanvonServices().AddFileWriters();

    // Blazor Services
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddBlazorContextMenu();
    // End of Blazer Services

    // Adding Timy service that runs websockets in background
    builder.Services.AddHostedService<WebTimyBackgroundService>().AddHostedService<HanvonService>();

    // Register ZKTecoController as a controller
    builder.Logging.SetMinimumLevel(LogLevel.Information);
    builder.Services.AddControllers().AddApplicationPart(typeof(ZkTecoController).Assembly);

    if (!builder.Environment.IsDevelopment()) builder.Logging.SetMinimumLevel(LogLevel.Information);
    builder.Logging.AddCustomLogging(logsPath, builder.Services);
    var app = builder.Build();

    var logger = LogManager.GetCurrentClassLogger();

    // If the application isn't running as dev we specify an exception handler
    // And enable HSTS.
    // HSTS Doesn't serve much purpose at the moment as we don't have an SSL Certificate
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    else
    {
        // Checks if EvoComms has been launched as a console application.
        // If the EvoComms service is already running, it logs an error message 
        // and exits the application to prevent port conflicts, as the service
        // will already be using the ports required by EvoComms
        if (!isService)
            try
            {
                using (var serviceController = new ServiceController("EvoCommsService"))
                {
                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        logger.Error(
                            "EvoComms is already running as a service. Please first stop the service before running Debug Console.");
                        return; // Exit the application
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // Log that the service does not exist 
                logger.Info("The EvoCommsService does not exist. Continuing in console mode.");
            }
    }


    // Https Redirection doesn't yet work as no SSL Certificate has been created
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    // Map Controllers is required to map the previously registered ZKTecoController
    app.MapControllers();
    app.UseRouting();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
    logger.Info("Starting application...");
    app.Run();
}
catch (Exception exception)
{
    var logger = LogManager.GetCurrentClassLogger();
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}