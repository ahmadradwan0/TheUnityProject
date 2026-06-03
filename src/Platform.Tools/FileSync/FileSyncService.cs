using Platform.Core.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Tools.FileSync
{
    public class FileSyncService(IEnumerable<ISyncStrategy> syncStrategies)
    {
        public Result<bool> Sync(SyncType syncType, string src, string dest)
        {
            ISyncStrategy? strategy = syncStrategies.FirstOrDefault(sy => sy.Type == syncType);

            return strategy is not null
                ? strategy.Sync(src, dest)
                : Result<bool>.Failure($"No strategy found for: {syncType}");
        }
    }
}
