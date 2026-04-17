using System.CommandLine;
namespace Platform.Core.Abstractions;

public interface ICommandModule
{
    Command Build();
}