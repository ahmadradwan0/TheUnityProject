using Platform.Core.Result;
using Platform.Infrastructure.FileSystem;
using Platform.Infrastructure.Hashing;
using System.Text;

namespace Platform.Tools.FileSync
{
    public class LocalSync(IFileSystem fileSystem) : ISyncStrategy
    {
        public SyncType Type => SyncType.Local;

        public Result<bool> Sync(string src, string dest)
        {
			try
			{
                PathType srcType = fileSystem.Exists(src);
                PathType destType = fileSystem.Exists(dest);

                if (srcType == PathType.NotFound)
                {
                    return Result<bool>.Failure($"Source not found or not a file: {src}");
                }

                if (Path.GetFullPath(src) == Path.GetFullPath(dest))
                {
                    return Result<bool>.Failure("Source and destination are the same");
                }

                if (srcType == PathType.File)
                {
                    return SyncFile(src, dest, destType);
                }

                if (srcType == PathType.Directory)
                {
                    return SyncDirectory(src, dest, destType);
                }
            }
			catch (Exception e)
			{
                return Result<bool>.Failure(e.Message);
			}

            return Result<bool>.Failure("Unexpected path type");
        }

        private Result<bool> SyncFile(string src, string dest, PathType destType)
        {
            Result<string> srcContent;
            Result<string> destContent;

            string srcFileHash = string.Empty;
            string destFileHash = string.Empty;

            // copy file
            if (destType == PathType.File)
            {
                // copy file cause both exsists and both fiels 
                srcContent = fileSystem.Read(src);
                destContent = fileSystem.Read(dest);

                if (!srcContent.IsSuccess)
                {
                    return Result<bool>.Failure(srcContent.Error!);
                }

                if (!destContent.IsSuccess)
                {
                    return Result<bool>.Failure(destContent.Error!);
                }

                srcFileHash = HashService.ComputeHash(Encoding.UTF8.GetBytes(srcContent.Value!));
                destFileHash = HashService.ComputeHash(Encoding.UTF8.GetBytes(destContent.Value!));

                if (srcFileHash == destFileHash)
                {
                    return Result<bool>.Success(true);
                }
                else
                {
                    // copy source to dest cause we know both are same type and not the same 
                    Result<bool> copyResult = CopyAndVerify(src, dest);
                    if (!copyResult.IsSuccess)
                    {
                        return copyResult;
                    }

                    return Result<bool>.Success(true);
                }
            }
            
            if (destType == PathType.Directory)
            {
                // copy into directory
                string fileName = Path.GetFileName(src);
                string destPath = Path.Combine(dest, fileName);

                Result<bool> copyResult = CopyAndVerify(src, destPath);
                if (!copyResult.IsSuccess)
                {
                    return copyResult;
                }

                return Result<bool>.Success(true);
            }

            if (destType == PathType.NotFound)
            {
                Result<bool> copyResult = CopyAndVerify(src, dest);
                if (!copyResult.IsSuccess)
                {
                    return copyResult;
                }

                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure("Unexpected path type");
        }

        private Result<bool> SyncDirectory(string src, string dest, PathType destType)
        {
            if (destType == PathType.Directory)
            {
                // sync files
                foreach (string file in Directory.GetFiles(src))
                {
                    string fileName = Path.GetFileName(file);
                    string destPath = Path.Combine(dest, fileName);
                    Result<bool> result = SyncFile(file, destPath, fileSystem.Exists(destPath));
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                }

                // Then — sync all subdirectories
                foreach (string dir in Directory.GetDirectories(src))
                {
                    string dirName = Path.GetFileName(dir);
                    string destDir = Path.Combine(dest, dirName);
                    Result<bool> result = SyncDirectory(dir, destDir, fileSystem.Exists(destDir));
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                }

                return Result<bool>.Success(true);
            }

            if (destType == PathType.NotFound)
            {
                Directory.CreateDirectory(dest);
                foreach (string file in Directory.GetFiles(src))
                {
                    string fileName = Path.GetFileName(file);
                    string destPath = Path.Combine(dest, fileName);
                    Result<bool> result = SyncFile(file, destPath, fileSystem.Exists(destPath));
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                }

                // Then — sync all subdirectories
                foreach (string dir in Directory.GetDirectories(src))
                {
                    string dirName = Path.GetFileName(dir);
                    string destDir = Path.Combine(dest, dirName);
                    Result<bool> result = SyncDirectory(dir, destDir, fileSystem.Exists(destDir));
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                }

                return Result<bool>.Success(true);
            }

            if (destType == PathType.File)
            {
                return Result<bool>.Failure("Cannot sync a directory to a file path");
            }

            return Result<bool>.Failure("Unexpected path type");
        }

        private Result<bool> CopyAndVerify(string src, string dest, bool overwrite = true)
        {
            try
            {
                File.Copy(src, dest, overwrite);

                Result<string> srcContent = fileSystem.Read(src);
                Result<string> destContent = fileSystem.Read(dest);

                if (!srcContent.IsSuccess)
                {
                    return Result<bool>.Failure($"Cannot read source after copy: {srcContent.Error}");
                }

                if (!destContent.IsSuccess)
                {
                    return Result<bool>.Failure($"Cannot read destination after copy: {destContent.Error}");
                }

                string srcHash = HashService.ComputeHash(Encoding.UTF8.GetBytes(srcContent.Value!));
                string destHash = HashService.ComputeHash(Encoding.UTF8.GetBytes(destContent.Value!));

                if (srcHash != destHash)
                {
                    return Result<bool>.Failure($"Verification failed: {dest}");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                return Result<bool>.Failure(e.Message);
            }
        }
    }
}
