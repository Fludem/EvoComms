namespace EvoComms.Web.App.TLS;

public static class HostsFileManager
{
    private const string HostsFile = @"C:\Windows\System32\drivers\etc\hosts";
    private const string DomainName = "evocomms.ui";

    public static void AddHostsEntry()
    {
        try
        {
            var hostsContent = File.ReadAllLines(HostsFile).ToList();
            var entry = $"127.0.0.1 {DomainName}";

            // Remove existing entry if present
            hostsContent.RemoveAll(line =>
                !line.StartsWith("#") && line.Contains(DomainName));

            // Add new entry
            hostsContent.Add(entry);

            // Write back to hosts file
            File.WriteAllLines(HostsFile, hostsContent);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to modify hosts file. Ensure running with admin privileges.", ex);
        }
    }

    public static void RemoveHostsEntry()
    {
        try
        {
            var hostsContent = File.ReadAllLines(HostsFile)
                .Where(line => !line.Contains(DomainName))
                .ToArray();

            File.WriteAllLines(HostsFile, hostsContent);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to modify hosts file. Ensure running with admin privileges.", ex);
        }
    }
}