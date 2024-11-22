namespace EvoComms.Devices.HanvonVF.Encryption;

// ReSharper disable once InconsistentNaming
public class SM2Keys
{
    public SM2Keys(string privateKey, string publicKeyX, string publicKeyY)
    {
        PrivateKey = privateKey;
        PublicKeyX = publicKeyX;
        PublicKeyY = publicKeyY;
    }

    public string PrivateKey { get; }
    public string PublicKeyX { get; }
    public string PublicKeyY { get; }
}