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
        var handlerTypes = typeof(IHanvonMessageHandler).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract &&
                        !t.IsInterface &&
                        typeof(IHanvonMessageHandler).IsAssignableFrom(t) &&
                        t.GetCustomAttribute<HanvonCommandHandlerAttribute>() != null);

        foreach (var handlerType in handlerTypes) services.AddSingleton(typeof(IHanvonMessageHandler), handlerType);

        services.AddSingleton<HanvonSettingsProvider>();
        return services;
    }
}