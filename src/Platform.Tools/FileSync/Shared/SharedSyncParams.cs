using System.CommandLine;
namespace Platform.Tools.FileSync.Shared;

public class SharedSyncParams
{
    public Argument<string> Source { get; } = new("source");
    public Argument<string> Destination { get; } = new("destination");
    public Option<bool> Verify { get; } = new("--verify");
    public Option<bool> Overwrite { get; } = new("--overwrite");

    public virtual List<Argument> Arguments => [Source, Destination];
    public virtual List<Option> Options => [Verify, Overwrite];
}

public class LocalSyncParams : SharedSyncParams
{
    // inherits everything, nothing extra for now
}

public class SshSyncParams : SharedSyncParams
{
    public Option<string> Host { get; } = new("--host");
    public Option<string> Username { get; } = new("--username");
    public Option<string> Password { get; } = new("--Password");

    public override List<Option> Options => [.. base.Options, Host, Username];
}