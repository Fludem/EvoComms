using EvoComms.Devices.HanvonVF.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace EvoComms.Devices.HanvonVF;

public static class HanvonServiceExtensions
{
    public static IServiceCollection AddHanvonServices(this IServiceCollection services)
    {
        services.AddSingleton<HanvonSettingsProvider>();
        return services;
    }
}