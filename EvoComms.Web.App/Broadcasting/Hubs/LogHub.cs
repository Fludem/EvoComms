using EvoComms.Logging.Events;
using Microsoft.AspNetCore.SignalR;

namespace EvoComms.Web.App.Broadcasting.Hubs;

public class LogHub : Hub
{
    public LogHub()
    {
        NotifyNewLogAdded.NewLogEntryAdded += OnNewLogEntry;
    }

    public async void OnNewLogEntry(object? sender, NewLogEntryEventArgs e)
    {
        await Clients.All.SendAsync("NewLogEntry", e.LogEntry);
    }
}