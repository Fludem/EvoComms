namespace EvoComms.Devices.Timy.Messages.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ResponseHandlerAttribute : Attribute
{
    public ResponseHandlerAttribute(string responseType)
    {
        ResponseType = responseType;
    }

    public string ResponseType { get; }
}