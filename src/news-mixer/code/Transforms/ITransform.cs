using NewsMixer.Models;

namespace NewsMixer.Transforms
{
    public interface ITransform
    {
        public IAsyncEnumerable<NewsItem> Execute(IAsyncEnumerable<NewsItem> items, CancellationToken cancellationToken);
    }
}
