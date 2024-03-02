using NewsMixer.Models;

namespace NewsMixer.Output.Console
{
    public class ConsoleOutput : IOutput
    {
        private readonly string _prefix;

        public ConsoleOutput(string prefix = "")
        {
            _prefix = prefix;
        }

        public Task Execute(NewsItem itm, CancellationToken cancellationToken = default)
        {
            System.Console.WriteLine($"{_prefix}{itm.Date:O}: {itm.Title}\n{itm.Content}");
            return Task.CompletedTask;
        }
    }
}
