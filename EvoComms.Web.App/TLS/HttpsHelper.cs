using System.Security.Cryptography.X509Certificates;

namespace EvoComms.Web.App.TLS;

public static class HttpsHelper
{
    public static void SetupHttps(string programDataFolder)
    {
        var certFolder = GetCertificateFolder(programDataFolder);
        Console.WriteLine($"Setting up HTTPS with self signed certificate at: {certFolder}");
        var cert = EnsureHttpsCertificate(certFolder);
        HostsFileManager.AddHostsEntry();
    }

    private static string GetCertificateFolder(string programDataFolder)
    {
        var certStore = Path.Combine(programDataFolder, "Certstore");
        Directory.CreateDirectory(certStore);
        return certStore;
    }

    public static X509Certificate2 EnsureHttpsCertificate(string programDataFolder)
    {
        var certFolder = GetCertificateFolder(programDataFolder);
        var certFile = Path.Combine(certFolder, "cert.pfx");
        if (File.Exists(certFolder)) return new X509Certificate2(certFile);

        var cert = CertificateManager.GenerateSelfSignedCertificate();
        Directory.CreateDirectory(Path.GetDirectoryName(certFile) ??
                                  throw new FileLoadException(
                                      $"Could not create directory to store Self Signed Cert for HTTPS at: {certFile}"));
        File.WriteAllBytes(certFile, cert.Export(X509ContentType.Pfx));
        CertificateManager.InstallCertificate(cert);
        return cert;
    }
}