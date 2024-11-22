using System.Diagnostics;

namespace EvoComms.Web.App.Services.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureHost(this IHostBuilder hostBuilder, bool isService)
    {
        if (isService)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            hostBuilder.UseContentRoot(pathToContentRoot!);
            hostBuilder.UseWindowsService(options => options.ServiceName = "EvoCommsService");
        }

        return hostBuilder;
    }
}