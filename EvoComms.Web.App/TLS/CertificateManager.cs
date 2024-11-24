using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace EvoComms.Web.App.TLS;

public static class CertificateManager
{
    private const string DomainName = "evocomms.ui";
    private const string CertificatePassword = "05QQ^CUR+t0K";
    private static readonly X500DistinguishedName DistinguishedName = new($"CN={DomainName}");

    public static X509Certificate2 GenerateSelfSignedCertificate()
    {
        using var rsa = RSA.Create(2048);

        var request = new CertificateRequest(
            DistinguishedName,
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        // Add Subject Alternative Name (SAN)
        var sanBuilder = new SubjectAlternativeNameBuilder();
        sanBuilder.AddDnsName(DomainName);
        sanBuilder.AddDnsName("localhost");
        request.CertificateExtensions.Add(sanBuilder.Build());

        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(
                X509KeyUsageFlags.DataEncipherment |
                X509KeyUsageFlags.KeyEncipherment |
                X509KeyUsageFlags.DigitalSignature,
                true));

        request.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension(
                new OidCollection
                {
                    new Oid("1.3.6.1.5.5.7.3.1") // Server Authentication
                },
                true));

        var certificate = request.CreateSelfSigned(
            DateTimeOffset.Now.AddDays(-1),
            DateTimeOffset.Now.AddYears(10));

        /**
         * @TODO: Need to think about security implications of this.
         */
        return new X509Certificate2(certificate.Export(X509ContentType.Pfx),
            CertificatePassword,
            X509KeyStorageFlags.MachineKeySet |
            X509KeyStorageFlags.PersistKeySet |
            X509KeyStorageFlags.Exportable);
    }

    public static void InstallCertificate(X509Certificate2 certificate)
    {
        using var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
        store.Open(OpenFlags.ReadWrite);
        store.Add(certificate);
        store.Close();
    }
}