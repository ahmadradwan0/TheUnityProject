namespace Platform.Core.Abstractions
{
    public interface ITool
    {
        string Name { get; }
        string Description { get; }
        IEnumerable<ICommandModule> CommandModules { get; }
    }
}
