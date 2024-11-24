using System.Reflection;
using EvoComms.Devices.HanvonVF.HanvonServer;
using EvoComms.Devices.HanvonVF.Messages.Handlers;
using EvoComms.Devices.HanvonVF.Messages.Handlers.Commands.Attributes;
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