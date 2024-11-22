using EvoComms.Logging.Targets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace EvoComms.Logging.Services;

public static class LoggingServiceExtensions
{
    public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder loggingBuilder, string logsPath,
        IServiceCollection serviceCollection)
    {
        ConsoleTarget consoleTarget = new("console")
        {
            Layout =
                "${longdate}|${level:uppercase=true}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };

        FileTarget fileTarget = new("file")
        {
            FileName = Path.Combine(logsPath, "evocomms-${date:format=dd-MM-yyyy}.log"),
            Layout =
                "${longdate}|${level:uppercase=true}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}",
            ArchiveFileName = Path.Combine(logsPath, "archive", "timy-worker-{#}.log"),
            ArchiveNumbering = ArchiveNumberingMode.Date,
            ArchiveEvery = FileArchivePeriod.Day,
            MaxArchiveFiles = 14
        };
        var databaseTargetNlog = new DatabaseTargetNlog(serviceCollection.BuildServiceProvider())
        {
            Layout =
                "${longdate}|${level:uppercase=true}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };

        LoggingConfiguration logConfig = new();
        logConfig.AddTarget(consoleTarget);
        logConfig.AddRuleForOneLevel(NLog.LogLevel.Info, consoleTarget, "EvoComms.*");
        logConfig.AddTarget(fileTarget);
        logConfig.AddRuleForOneLevel(NLog.LogLevel.Info, fileTarget, "EvoComms.*");
        logConfig.AddTarget(databaseTargetNlog);
        logConfig.AddRuleForOneLevel(NLog.LogLevel.Info, databaseTargetNlog, "EvoComms.*");
        logConfig.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, fileTarget, "Microsoft.AspNetCore.*");
        logConfig.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, consoleTarget, "Microsoft.AspNetCore.*");
        logConfig.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, databaseTargetNlog, "Microsoft.AspNetCore.*");

        LogManager.Configuration = logConfig;

        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(LogLevel.Information);
        loggingBuilder.AddNLog(logConfig);

        return loggingBuilder;
    }
}