using Platform.Core.Enums;
using Platform.Core.Result;

namespace Platform.Infrastructure.FileSystem
{
    public interface IFileSystem
    {
        public Result<string> Read(string file);
        public Result<string> Write(string file, string data);
        public PathType Exists(string file);
    }
}
 