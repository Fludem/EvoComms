using System;

using EvoComms.Core.Filesystem.Writers.BioTime;
using EvoComms.Core.Filesystem.Writers.InfoTime;
using EvoComms.Core.Filesystem.Writers.InTime;
using EvoComms.Core.Interfaces;
using EvoComms.Core.Models.Enums;

using Microsoft.Extensions.DependencyInjection;

namespace EvoComms.Core.Filesystem.Writers
{
    public class ClockingWriterFactory : IClockingWriterFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ClockingWriterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IClockingFileWriter GetWriter(OutputType outputType)
        {
            return outputType switch
            {
                OutputType.InfoTime => _serviceProvider.GetRequiredService<InfoTimeWriter>(),
                OutputType.BioTime => _serviceProvider.GetRequiredService<BioTimeWriter>(),
                OutputType.TotalTime => _serviceProvider.GetRequiredService<BioTimeWriter>(),
                OutputType.InTime => _serviceProvider.GetRequiredService<InTimeWriter>(),
                _ => throw new ArgumentException("Invalid output type", nameof(outputType))
            };
        }
    }
}