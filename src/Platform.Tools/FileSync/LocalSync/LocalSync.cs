using Platform.Core.Result;
using Platform.Infrastructure.FileSystem;
using Platform.Infrastructure.Hashing;
using Platform.Tools.FileSync.Services;
using System.Text;

namespace Platform.Tools.FileSync.LocalSync
{
    public class LocalSync(IFileSystem fileSystem) : ISyncStrategy
    {
        public SyncType Type => SyncType.Local;

        public Result<bool> Sync(SyncSettings settings)
        {
			try
			{
                PathType srcType = fileSystem.Exists(settings.Source);
                PathType destType = fileSystem.Exists(settings.Destination);

                if (srcType == PathType.NotFound)
                {
                    return Result<bool>.Failure($"Source not found or not a file: {settings.Source}");
                }

                if (Path.GetFullPath(settings.Source) == Path.GetFullPath(settings.Destination))
                {
                    return Result<bool>.Failure("Source and destination are the same");
                }

                if (srcType == PathType.File)
                {
                    return SyncFile(settings.Source, settings.Destination, destType, settings);
                }

                if (srcType == PathType.Directory)
                {
                    return SyncDirectory(settings.Source, settings.Destination, destType, settings);
                }
            }
			catch (Exception e)
			{
                return Result<bool>.Failure(e.Message);
			}

            return Result<bool>.Failure("Unexpected path type");
        }

        private Result<bool> SyncFile(string src, string dest, PathType destType, SyncSettings settings)
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
                    Result<bool> copyResult = SyncHelper.CopyAndVerify(src, dest, fileSystem, settings);
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

                Result<bool> copyResult = SyncHelper.CopyAndVerify(src, destPath, fileSystem, settings);
                if (!copyResult.IsSuccess)
                {
                    return copyResult;
                }

                return Result<bool>.Success(true);
            }

            if (destType == PathType.NotFound)
            {
                Result<bool> copyResult = SyncHelper.CopyAndVerify(src, dest, fileSystem, settings);
                if (!copyResult.IsSuccess)
                {
                    return copyResult;
                }

                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure("Unexpected path type");
        }

        private Result<bool> SyncDirectory(string src, string dest, PathType destType, SyncSettings settings)
        {
            if (destType == PathType.Directory)
            {
                // sync files
                foreach (string file in Directory.GetFiles(src))
                {
                    string fileName = Path.GetFileName(file);
                    string destPath = Path.Combine(dest, fileName);
                    Result<bool> result = SyncFile(file, destPath, fileSystem.Exists(destPath), settings);
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
                    Result<bool> result = SyncDirectory(dir, destDir, fileSystem.Exists(destDir), settings);
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
                    Result<bool> result = SyncFile(file, destPath, fileSystem.Exists(destPath), settings);
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
                    Result<bool> result = SyncDirectory(dir, destDir, fileSystem.Exists(destDir), settings);
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

    }
}
