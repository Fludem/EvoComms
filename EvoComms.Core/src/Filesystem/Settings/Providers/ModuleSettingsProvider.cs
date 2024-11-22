using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace EvoComms.Core.Filesystem.Settings.Providers
{
    public abstract class ModuleSettingsProvider<T> : IModuleSettingsProvider where T : ModuleSettings
    {
        protected readonly string _fileName;
        protected readonly ILogger _logger;
        protected T? _settings;

        protected ModuleSettingsProvider(string fileName, ILogger logger)
        {
            _fileName = fileName;
            _logger = logger;
        }

        public async Task<int> GetPort()
        {
            _settings ??= await LoadSettings();

            return _settings.ListenPort;
        }

        public async Task<string> GetOutputPath()
        {
            _settings ??= await LoadSettings();

            return _settings.OutputPath;
        }

        public async Task<bool> IsEnabled()
        {
            _settings ??= await LoadSettings();
            return _settings.Enabled;
        }

        protected abstract T CreateDefaultSettings();

        public async Task<T> LoadSettings()
        {
            string settingsPath =
                Path.Combine("C:", "ProgramData", "ClockingSystems", "EvoComms", "Settings", _fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(settingsPath) ?? throw new InvalidOperationException());

            try
            {
                if (File.Exists(settingsPath))
                {
                    string json = await File.ReadAllTextAsync(settingsPath);
                    return JsonSerializer.Deserialize<T>(json) ?? CreateDefaultSettings();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading settings");
            }

            return CreateDefaultSettings();
        }

        public async Task SaveSettings(T settings)
        {
            string settingsPath =
                Path.Combine("C:", "ProgramData", "ClockingSystems", "EvoComms", "Settings", _fileName);
            try
            {
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(settingsPath, json);
                _settings = settings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving settings");
            }
        }
    }
}