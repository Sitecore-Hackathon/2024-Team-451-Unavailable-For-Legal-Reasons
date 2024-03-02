using NewsMixer.Models;

namespace NewsMixer.Transforms.DummySummary
{
    public class DummySummaryTransform : ITransform
    {
        public Task<NewsItem> Execute(NewsItem item, CancellationToken cancellationToken)
        {
            item.Content = item.Content[..200];

            return Task.FromResult(item);
        }
    }
}
