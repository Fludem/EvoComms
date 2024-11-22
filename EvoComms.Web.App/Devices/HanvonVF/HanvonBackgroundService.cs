namespace EvoComms.Web.App.Devices.HanvonVF;

public class HanvonBackgroundService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
        }

        throw new NotImplementedException();
    }
}