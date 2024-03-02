using NewsMixer.Models;

namespace NewsMixer.Output.Console
{
    public class ConsoleOutput(string prefix = "") : IOutput
    {
        public Task Execute(NewsItem itm, CancellationToken cancellationToken = default)
        {
            System.Console.WriteLine($"{prefix}{itm.Date:yyyy-MM-dd}: {itm.Title}\n{itm.Content}");
            return Task.CompletedTask;
        }
    }
}
