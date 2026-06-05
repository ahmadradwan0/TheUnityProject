using Platform.Core.Result;

namespace Platform.Tools.FileSync
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
