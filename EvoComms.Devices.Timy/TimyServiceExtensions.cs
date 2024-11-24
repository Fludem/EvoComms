using System.Reflection;
using EvoComms.Core.Services;
using EvoComms.Devices.Timy.Messages;
using EvoComms.Devices.Timy.Messages.Attributes;
using EvoComms.Devices.Timy.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace EvoComms.Devices.Timy;

public static class TimyServiceExtensions
{
    public static IServiceCollection AddTimyServices(this IServiceCollection services)
    {
        // Register core services
        services.AddSingleton<TimySettingsProvider>();
        services.AddSingleton<RecordService>();
        services.AddSingleton<TimyHandlerRegistry>();
        services.AddSingleton<TimyMessageProcessor>();
        services.AddSingleton<TimyListenerFactory>();

        // Auto-register all handlers from the assembly
        var handlerTypes = typeof(IMessageHandler).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract &&
                        !t.IsInterface &&
                        typeof(IMessageHandler).IsAssignableFrom(t) &&
                        (t.GetCustomAttribute<TimyCommandHandlerAttribute>() != null ||
                         t.GetCustomAttribute<TimyResponseHandlerAttribute>() != null));

        foreach (var handlerType in handlerTypes) services.AddSingleton(typeof(IMessageHandler), handlerType);

        return services;
    }
}