using Platform.Core.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Tools.FileSync
{
    public interface ISyncStrategy
    {
        public Result<bool> Sync(string src, string dest);
        SyncType Type { get; }
    }
}
