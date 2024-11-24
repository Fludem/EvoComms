namespace EvoComms.Devices.Timy.Messages.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TimyResponseHandlerAttribute(string responseType) : Attribute
{
    public string ResponseType { get; } = responseType;
}