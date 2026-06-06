
using Platform.Core.Enums;
namespace Platform.Tools.FileSync.Shared;


public class SyncSettings
{
    public required string Source { get; init; }
    public required string Destination { get; init; }
    public bool Verify { get; init; } = false;
    public bool Overwrite { get; init; } = false;
    public HashType HashType { get; init; } = HashType.MD5;

}

public class LocalSyncSettings : SyncSettings
{
    public static SyncSettings Local(string source, string destination, bool verify = false, bool overwrite = false)
    {
        return new SyncSettings
        {
            Source = source,
            Destination = destination,
            Verify = verify,
            Overwrite = overwrite
        };
    }
}

public class RemoteSyncSettings : SyncSettings
{
    public required string Host { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }

    public static RemoteSyncSettings Remote(
        string source,
        string destination,
        string host,
        bool verify = false,
        bool overwrite = false,
        string? username = null,
        string? password = null)
    {
        return new RemoteSyncSettings
        {
            Source = source,
            Destination = destination,
            Host = host,
            Verify = verify,
            Overwrite = overwrite,
            Username = username,
            Password = password
        };
    }
}