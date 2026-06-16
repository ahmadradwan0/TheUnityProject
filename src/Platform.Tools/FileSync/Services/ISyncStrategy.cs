using Platform.Core.Result;

namespace Platform.Tools.FileSync.Services
{
    public interface ISyncStrategy
    {
        public Result<bool> Sync(SyncSettings settings);
        SyncType Type { get; }
    }
}
