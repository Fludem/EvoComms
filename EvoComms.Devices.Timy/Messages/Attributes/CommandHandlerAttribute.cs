namespace EvoComms.Devices.Timy.Messages.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandHandlerAttribute : Attribute
{
    public CommandHandlerAttribute(string command)
    {
        Command = command;
    }

    public string Command { get; }
}