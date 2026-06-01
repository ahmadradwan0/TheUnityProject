using Platform.Core.Result;

namespace Platform.Infrastructure.FileSystem
{
    public class FileSystem : IFileSystem
    {
        public Result<string> Read(string file)
        {
            try
            {
                using StreamReader sr = new(file);
                string data = sr.ReadToEnd();

                return Result<string>.Success(data);
            }
            catch (Exception e)
            {
                return Result<string>.Failure(e.Message);
            }
        }

        public Result<string> Write(string file, string data)
        {
            try
            {
                using StreamWriter sw = new(file);
                sw.Write(data);

                return Result<string>.Success("Done");
            }
            catch (Exception e)
            {
                return Result<string>.Failure(e.Message);
            }
        }

        public PathType Exists(string file)
        {
            if (File.Exists(file))
            {
                return PathType.File;
            }

            if (Directory.Exists(file))
            {
                return PathType.Directory;
            }

            return PathType.NotFound;
        }
    }
}
