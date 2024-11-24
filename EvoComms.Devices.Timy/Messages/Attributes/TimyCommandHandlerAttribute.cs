namespace EvoComms.Devices.Timy.Messages.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TimyCommandHandlerAttribute(string command) : Attribute
{
    public string Command { get; } = command;
}