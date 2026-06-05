using Platform.Core.Abstractions;
using Platform.Core.Builders;
using Platform.Core.Result;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace Platform.Tools.FileSync
{
    public class FileSyncCommandModule(FileSyncService fileSyncService) : ICommandModule
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

            Argument<string> srcArg = new("source");
            Argument<string> destArg = new("destination");

            List<Argument> arguments = [srcArg, destArg];

            Option<bool> verifyOption = new("--verify");
            Option<bool> overwriteOption = new("--overwrite");

            List<Option> options = [verifyOption, overwriteOption];

            Command command = CommandBuilder.Create(
                "sync",
                "sync files between locations",
                arguments: arguments,
                options: options
            );

            command.SetAction((parseResult) =>
            {
                string src = parseResult.GetValue(srcArg) ?? string.Empty;
                string dest = parseResult.GetValue(destArg) ?? string.Empty;

                Result<bool> result = fileSyncService.Sync(SyncType.Local, src, dest);
            });

            return 
        }
    }
}
