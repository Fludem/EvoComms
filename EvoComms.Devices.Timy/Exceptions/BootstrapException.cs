using System.Configuration;
using NLog;

namespace EvoComms.Devices.Timy.Exceptions;

public class BootstrapException : ConfigurationErrorsException
{
    public BootstrapException(string message) : base(message)
    {
        LogManager.GetCurrentClassLogger()
            .Error(
                $"Exception when bootstrapping web socket servers. | Message: {message}");
    }
}