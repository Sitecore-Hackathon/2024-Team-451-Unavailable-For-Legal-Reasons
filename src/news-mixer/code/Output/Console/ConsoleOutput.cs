using NewsMixer.Models;

namespace NewsMixer.Output.Console
{
    public class ConsoleOutput : IOutput
    {
        public async Task Execute(IAsyncEnumerable<NewsItem> items, CancellationToken cancellationToken = default)
        {
            await foreach (var itm in items)
            {
                System.Console.WriteLine($"{itm.Date}: {itm.Title}\n{itm.Content}");
            }
        }
    }
}
