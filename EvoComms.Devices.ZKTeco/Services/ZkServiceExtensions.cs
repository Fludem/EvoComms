using EvoComms.Devices.ZKTeco.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace EvoComms.Devices.ZKTeco.Services;

public static class ZkServiceExtensions
{
    public static IServiceCollection AddZkServices(this IServiceCollection services)
    {
        services.AddSingleton<ZkSettingsProvider>();
        return services;
    }
}