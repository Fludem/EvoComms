using EvoComms.Core.Filesystem.Writers;
using EvoComms.Core.Filesystem.Writers.BioTime;
using EvoComms.Core.Filesystem.Writers.InfoTime;
using EvoComms.Core.Filesystem.Writers.InTime;

using Microsoft.Extensions.DependencyInjection;

namespace EvoComms.Core.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileWriters(this IServiceCollection services)
        {
            services.AddSingleton<InfoTimeWriter>();
            services.AddSingleton<BioTimeWriter>();
            services.AddSingleton<InTimeWriter>();
            services.AddSingleton<IClockingWriterFactory, ClockingWriterFactory>();
            return services;
        }
    }
}