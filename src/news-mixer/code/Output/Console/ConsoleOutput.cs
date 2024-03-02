using Microsoft.Extensions.Logging;
using NewsMixer.Models;
using NewsMixer.Transforms.OpenAiSummary;

namespace NewsMixer.Output.Console
{
    public class ConsoleOutput(string prefix, ILogger logger) : IOutput
    {
        public Task Execute(NewsItem itm, CancellationToken cancellationToken = default)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("{prefix} {date} {title} {content}", prefix, itm.Date, itm.Title, itm.Content);
            }
            
            return Task.CompletedTask;
        }
    }
}
