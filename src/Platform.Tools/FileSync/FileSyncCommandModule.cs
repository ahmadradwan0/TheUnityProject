using Platform.Core.Abstractions;
using Platform.Core.Builders;
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
            List<Argument> arguments = [
                new Argument<string>("source"),
                new Argument<string>("destination")
            ];

            List<Option> options = [
                new Option<bool>("--verify"),
                new Option<bool>("--overwrite")
            ];

            return CommandBuilder.Create(
                "sync",
                "sync files between locations",
                arguments,
                options
            );
        }
    }
}
