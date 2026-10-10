using Platform.Core.Enums;
using Platform.Core.Result;
using Platform.Tools.FileSync.Shared;

namespace Platform.Tools.FileSync.Services
{
    public interface ISyncStrategy
    {
        public Result<bool> Sync(SyncSettings settings);
        SyncType Type { get; }
    }
}
