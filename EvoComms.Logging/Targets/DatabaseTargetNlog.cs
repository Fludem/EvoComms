using EvoComms.Core.Database;
using EvoComms.Core.Database.Models;
using EvoComms.Logging.Events;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Targets;

namespace EvoComms.Logging.Targets;

[Target("EFLogger")]
public class DatabaseTargetNlog : TargetWithLayout
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseTargetNlog(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Name = "Database";
    }

    protected override void Write(LogEventInfo logEvent)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logEntry = new LogEntry
        {
            TimeStamp = logEvent.TimeStamp,
            LogLevel = logEvent.Level.Name,
            Message = logEvent.FormattedMessage,
            Exception = logEvent.Exception?.ToString(),
            Logger = logEvent.LoggerName
        };

        dbContext.LogEntries.Add(logEntry);
        dbContext.SaveChanges();
        NotifyNewLogAdded.NotifyLogEntryAdded(logEntry);
    }
}