using NewsMixer.Models;

namespace NewsMixer.Transforms.DummySummary
{
    public class DummySummaryTransform : ITransform
    {
        public Task<NewsItem> Execute(NewsItem itm, CancellationToken cancellationToken)
        {
            itm.Content = itm.Content?[..200];
            return Task.FromResult(itm);
        }
    }
}
