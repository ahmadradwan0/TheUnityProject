
using Microsoft.Extensions.DependencyInjection;
using Platform.Core.Abstractions;
using Platform.Infrastructure.FileSystem;
using Platform.Infrastructure.Logging;
using Platform.Infrastructure.Processes;
using Platform.Tools.FileSync;
using Platform.Tools.FileSync.CommandModules;
using Platform.Tools.FileSync.LocalSync;
using Platform.Tools.FileSync.Services;

namespace Platform.Cli.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IAppLogger, AppLogger>();
            services.AddSingleton<IProcessRunner, ProcessRunner>();

            return services;
        }

        public static IServiceCollection AddFileSyncTool(this IServiceCollection services)
        {
            services.AddSingleton<ISyncStrategy, LocalSync>();
            services.AddSingleton<ITool, FileSyncTool>();
            services.AddSingleton<FileSyncService>();
            services.AddSingleton<FileSyncCommandModule>();

            return services;
        }
    }
}
