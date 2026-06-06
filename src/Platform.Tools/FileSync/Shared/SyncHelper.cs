using Platform.Core.Result;
using Platform.Infrastructure.FileSystem;
using Platform.Infrastructure.Hashing;
using System.Text;

public static class SyncHelper
{
    public static Result<bool> CopyAndVerify(
        string src,
        string dest,
        IFileSystem fileSystem,
        SyncSettings settings)
    {
        try
        {
            File.Copy(src, dest, settings.Overwrite);

            if (settings.Verify)
            {
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
            }

            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e.Message);
        }
    }
}