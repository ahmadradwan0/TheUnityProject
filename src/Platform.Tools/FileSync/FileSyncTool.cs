using Platform.Core.Abstractions;
using Platform.Tools.FileSync.CommandModules;
namespace Platform.Tools.FileSync
{
    public class FileSyncTool(FileSyncCommandModule fileSyncCommandModule) : ITool
    {
        public string Name => "sync";
        public string Description => "File synchronization tools";
        public IEnumerable<ICommandModule> CommandModules => [fileSyncCommandModule];
    }
}