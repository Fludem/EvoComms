namespace EvoComms.Devices.HanvonVF.Messages.Handlers.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HanvonCommandHandlerAttribute(string command) : Attribute
{
    public string Command { get; } = command;
}