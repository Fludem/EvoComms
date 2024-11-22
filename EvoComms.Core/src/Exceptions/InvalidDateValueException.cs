using System;

namespace EvoComms.Core.Exceptions
{
    public class InvalidDateValueException : Exception
    {
        public InvalidDateValueException(string propertyName, string? receivedValue)
            : base(
                $"Invalid date format for property '{propertyName}'. Received: '{receivedValue ?? "NULL VALUE"}'. Expected format: 'yyyy-MM-dd HH:mm:ss'")
        {
            PropertyName = propertyName;
            ReceivedValue = receivedValue;
            ExpectedFormat = "yyyy-MM-dd HH:mm:ss";
        }

        public InvalidDateValueException(string propertyName, string receivedValue, string message)
            : base(message)
        {
            PropertyName = propertyName;
            ReceivedValue = receivedValue;
            ExpectedFormat = "yyyy-MM-dd HH:mm:ss";
        }

        public InvalidDateValueException(string propertyName, string receivedValue, string message,
            Exception innerException)
            : base(message, innerException)
        {
            PropertyName = propertyName;
            ReceivedValue = receivedValue;
            ExpectedFormat = "yyyy-MM-dd HH:mm:ss";
        }

        public string PropertyName { get; }
        public string? ReceivedValue { get; }
        public string ExpectedFormat { get; }
    }
}