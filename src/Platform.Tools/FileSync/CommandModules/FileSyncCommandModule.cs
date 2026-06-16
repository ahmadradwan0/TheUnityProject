using Platform.Core.Abstractions;
using Platform.Core.Builders;
using Platform.Core.Logger;
using Platform.Core.Result;
using Platform.Infrastructure.Logging;
using Platform.Tools.FileSync.Services;
using System.CommandLine;

namespace Platform.Tools.FileSync.CommandModules
{
    public class FileSyncCommandModule(
        FileSyncService fileSyncService,
        IAppLogger logger
    ) : ICommandModule
    {
        public Command Build()
        {
            Command syncCommand = new("sync", "Sync Files between locations")
            {
                BuildLocalCommand(),
                //BuildSshCommand()
            };

            return syncCommand;
        }

        private Command BuildLocalCommand()
        {
            SharedSyncParams syncParams = new LocalSyncParams();

            Command command = CommandBuilder.Create(
                "local",
                "sync files between locations",
                arguments: syncParams.Arguments,
                options: syncParams.Options
            );

            command.SetAction((parseResult) =>
            {
                SyncSettings settings = LocalSyncSettings.Local(
                        parseResult.GetValue(syncParams.Source) ?? String.Empty,
                        parseResult.GetValue(syncParams.Destination) ?? String.Empty,
                        parseResult.GetValue(syncParams.Verify),
                        parseResult.GetValue(syncParams.Overwrite)
                    );

                Result<bool> result = fileSyncService.Sync(SyncType.Local, settings);

                if (result.IsSuccess)
                {
                    logger.ConsoleLog("Local sync completed successfully", LogLevel.Information);
                }
                else
                {
                    logger.ConsoleLog($"Local sync failed: {result.Error}", LogLevel.Error);
                }
            });

            return command;
        }
    }
}
