
using Microsoft.Extensions.DependencyInjection;
using Platform.Cli.Extensions;
using Platform.Core.Abstractions;
using System.CommandLine;

ServiceCollection services = new();
services.AddInfrastructure();
services.AddFileSyncTool();

ServiceProvider provider = services.BuildServiceProvider();

IEnumerable<ITool> tools = provider.GetServices<ITool>();

RootCommand rootCommand = new("Unity CLI Toolkit");

foreach (ITool tool in tools)
{
    foreach (ICommandModule module in tool.CommandModules)
    {
        rootCommand.Add(module.Build());
    }
}

rootCommand.Parse(args).Invoke();