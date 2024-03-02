using NewsMixer.Models;

namespace NewsMixer.Output
{
    public interface IOutput
    {
        public Task Execute(IAsyncEnumerable<NewsItem> items, CancellationToken cancellationToken = default);
    }
}
