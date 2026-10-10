using Platform.Core.Enums;
using Platform.Core.Result;
using Platform.Tools.FileSync.Shared;

namespace Platform.Tools.FileSync.Services
{
    public class FileSyncService(IEnumerable<ISyncStrategy> syncStrategies)
    {
        public Result<bool> Sync(SyncType syncType, SyncSettings settings)
        {
            ISyncStrategy? strategy = syncStrategies.FirstOrDefault(sy => sy.Type == syncType);

            return strategy is not null
                ? strategy.Sync(settings)
                : Result<bool>.Failure($"No strategy found for: {syncType}");
        }
    }
}
