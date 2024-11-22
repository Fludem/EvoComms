using System.Security.Cryptography;
using Com.FirstSolver.Splash;

namespace EvoComms.Devices.HanvonVF.Encryption;

// ReSharper disable once InconsistentNaming
public static class SM2KeyFactory
{
    private static string BytesToHex(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "").ToUpper();
    }


    private static SM2Keys CreateKeys()
    {
        var privateKeyBytes = RandomNumberGenerator.GetBytes(32);
        var publicKeyXBytes = RandomNumberGenerator.GetBytes(32);
        var publicKeyYBytes = RandomNumberGenerator.GetBytes(32);

        return new SM2Keys(
            BytesToHex(privateKeyBytes),
            BytesToHex(publicKeyXBytes),
            BytesToHex(publicKeyYBytes)
        );
    }

    public static SM2UserInformation CreateServerInfo(string ownerId)
    {
        var keys = CreateKeys();
        return new SM2UserInformation(
            ownerId,
            keys.PrivateKey,
            keys.PublicKeyX,
            keys.PublicKeyY
        );
    }
}