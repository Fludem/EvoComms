namespace EvoComms.Devices.HanvonVF.SM4Encryption;

// ReSharper disable once InconsistentNaming
public class SM2Keys(string privateKey, string publicKeyX, string publicKeyY)
{
    public string PrivateKey { get; } = privateKey;
    public string PublicKeyX { get; } = publicKeyX;
    public string PublicKeyY { get; } = publicKeyY;
}