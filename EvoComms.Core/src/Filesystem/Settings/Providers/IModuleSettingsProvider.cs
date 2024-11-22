using System.Threading.Tasks;

namespace EvoComms.Core.Filesystem.Settings.Providers
{
    public interface IModuleSettingsProvider
    {
        Task<string> GetOutputPath();
        Task<bool> IsEnabled();
        Task<int> GetPort();
    }
}