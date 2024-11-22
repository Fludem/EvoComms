namespace EvoComms.Devices.HanvonVF.Messages;

public interface IHanvonResponseMessage<TParam>
{
    string RETURN { get; }
    TParam PARAM { get; }
}