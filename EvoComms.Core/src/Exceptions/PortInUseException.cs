using System;

namespace EvoComms.Core.Exceptions
{
    public class PortInUseException : Exception
    {
        public PortInUseException(int port, string message)
            : base(message)
        {
            Port = port;
        }

        public int Port { get; }
    }
}