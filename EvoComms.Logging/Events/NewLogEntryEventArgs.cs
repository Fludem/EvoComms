using EvoComms.Core.Database.Models;

namespace EvoComms.Logging.Events;

public class NewLogEntryEventArgs : EventArgs
{
    public NewLogEntryEventArgs(LogEntry logEntry)
    {
        LogEntry = logEntry;
    }

    public LogEntry LogEntry { get; }
}