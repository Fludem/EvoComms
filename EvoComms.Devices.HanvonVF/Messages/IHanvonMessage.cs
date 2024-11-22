namespace EvoComms.Devices.HanvonVF.Messages;

public interface IHanvonMessage<TParam>
{
    string COMMAND { get; }
    TParam PARAM { get; }
}