using NewsMixer.Models;

namespace NewsMixer.Transforms
{
    public interface ITransform
    {
        public Task<NewsItem> Execute(NewsItem items, CancellationToken cancellationToken);
    }
}
