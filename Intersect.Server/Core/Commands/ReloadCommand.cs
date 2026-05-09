using Intersect.Core;
using Intersect.Server.Core.CommandParsing;
using Intersect.Server.Localization;
using Microsoft.Extensions.Logging;

namespace Intersect.Server.Core.Commands;

internal sealed partial class ReloadCommand : ServerCommand
{
    public ReloadCommand() : base(Strings.Commands.Reload)
    {
    }

    protected override void HandleValue(ServerContext context, ParserResult result)
    {
        try
        {
            var reloadResult = Options.ReloadLiveFromDisk();

            Console.WriteLine($@"    {Strings.Commandoutput.ReloadSucceeded}");

            if (reloadResult.AppliedChanges.Count > 0)
            {
                Console.WriteLine($@"    {Strings.Commandoutput.ReloadAppliedHeader}");
                foreach (var appliedChange in reloadResult.AppliedChanges)
                {
                    Console.WriteLine($@"      - {appliedChange}");
                }
            }
            else
            {
                Console.WriteLine($@"    {Strings.Commandoutput.ReloadNoReloadableChanges}");
            }

            if (reloadResult.RestartRequiredChanges.Count <= 0)
            {
                return;
            }

            Console.WriteLine($@"    {Strings.Commandoutput.ReloadRestartRequiredHeader}");
            foreach (var restartRequiredChange in reloadResult.RestartRequiredChanges)
            {
                Console.WriteLine($@"      - {restartRequiredChange}");
            }
        }
        catch (Exception exception)
        {
            ApplicationContext.Context.Value?.Logger.LogError(exception, "Failed to reload server configuration");
            Console.WriteLine($@"    {Strings.Commandoutput.ReloadFailed.ToString(exception.Message)}");
        }
    }
}
