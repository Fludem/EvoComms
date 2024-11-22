using EvoComms.Core.Models.Enums;

namespace EvoComms.Core.Filesystem.Settings
{
    public class ModuleSettings
    {
        public int ListenPort { get; set; } = 9922;
        public bool Enabled { get; set; } = false;
        public required string OutputPath { get; set; }
        public OutputType OutputType { get; set; } = OutputType.BioTime;
        public bool IsTemplateSharingEnabled { get; set; } = true;
    }
}