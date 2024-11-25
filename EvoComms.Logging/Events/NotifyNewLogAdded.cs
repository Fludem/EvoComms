using EvoComms.Core.Database.Models;

namespace EvoComms.Logging.Events;

public static class NotifyNewLogAdded
{
    public static event EventHandler<NewLogEntryEventArgs>? NewLogEntryAdded;

    public static void NotifyLogEntryAdded(LogEntry logEntry)
    {
        NewLogEntryAdded?.Invoke(null, new NewLogEntryEventArgs(logEntry));
    }
}