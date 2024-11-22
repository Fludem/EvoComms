using EvoComms.Core.Interfaces;
using EvoComms.Core.Models.Enums;

namespace EvoComms.Core.Filesystem.Writers
{
    public interface IClockingWriterFactory
    {
        IClockingFileWriter GetWriter(OutputType outputType);
    }
}