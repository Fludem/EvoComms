using System.Reflection;
using EvoComms.Devices.Timy.Messages;
using EvoComms.Devices.Timy.Messages.Attributes;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace EvoComms.Devices.Timy.ServiceExtensions;

public static class TimyServiceExtensions
{
    public static IServiceCollection AddTimyServices(this IServiceCollection services)
    {
        // Register core services
        services.AddSingleton<TimySettingsProvider>();
        services.AddSingleton<TimyHandlerRegistry>();
        services.AddSingleton<TimyMessageProcessor>();
        services.AddSingleton<TimyListenerFactory>();

        // Auto-register all handlers from the assembly
        var handlerTypes = typeof(ITimyMessageHandler).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract &&
                        !t.IsInterface &&
                        typeof(ITimyMessageHandler).IsAssignableFrom(t) &&
                        (t.GetCustomAttribute<TimyCommandHandlerAttribute>() != null ||
                         t.GetCustomAttribute<TimyResponseHandlerAttribute>() != null));

        foreach (var handlerType in handlerTypes) services.AddSingleton(typeof(ITimyMessageHandler), handlerType);

        return services;
    }
}