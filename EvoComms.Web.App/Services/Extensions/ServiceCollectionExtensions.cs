using EvoComms.Core.Database;
using EvoComms.Core.Services.Extensions;
using EvoComms.Devices.HanvonVF;
using EvoComms.Devices.Timy;
using EvoComms.Devices.ZKTeco.Http.Controllers;
using EvoComms.Devices.ZKTeco.Services;
using EvoComms.Logging.Services;
using EvoComms.Web.App.Devices.Timy;

namespace EvoComms.Web.App.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, string logsPath)
    {
        // Adding DB Context
        services.AddDbContext<AppDbContext>();
        // Blazor Services
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddBlazorContextMenu();

        ConfigureDeviceServices(services);
        AddBackgroundServices(services);

        // Register ZKTecoController as a controller
        services.AddControllers().AddApplicationPart(typeof(ZkTecoController).Assembly);

        return services;
    }

    public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder loggingBuilder, IHostEnvironment environment,
        string logsPath, IServiceCollection services)
    {
        loggingBuilder.SetMinimumLevel(environment.IsDevelopment() ? LogLevel.Debug : LogLevel.Information);
        loggingBuilder.AddCustomLogging(logsPath, services);
        return loggingBuilder;
    }

    private static void ConfigureDeviceServices(this IServiceCollection services)
    {
        services.AddHanvonServices().AddTimyServices().AddZkServices().AddFileWriters();
    }

    private static void AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<WebTimyBackgroundService>();
    }
}