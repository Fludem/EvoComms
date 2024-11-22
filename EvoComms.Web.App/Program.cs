using EvoComms.Web.App.Services.Extensions;
using NLog;

try
{
    var isService = !Environment.UserInteractive;

    var builder = WebApplication.CreateBuilder(args);

    /** The Windows Installer will place most things here such as settings and DB. */
    var programDataPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "ClockingSystems", "EvoComms"
    );

    var logsPath = Path.Combine(programDataPath, "Logs");
    Directory.CreateDirectory(logsPath);

    /** Configures host IE Content Root */
    builder.Host.ConfigureHost(isService);

    /** Configure Device Services, FIle Writers, Background Services etc... */
    builder.Services.ConfigureServices(logsPath);

    /** Configure logging for NLOG and Custom DB Target */
    builder.Logging.ConfigureLogging(builder.Environment, logsPath, builder.Services);

    var app = builder.Build();

    /** Ensures service isn't running (port conflict), and other blazor required things */
    app.ConfigureApp(isService);

    var logger = LogManager.GetCurrentClassLogger();
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