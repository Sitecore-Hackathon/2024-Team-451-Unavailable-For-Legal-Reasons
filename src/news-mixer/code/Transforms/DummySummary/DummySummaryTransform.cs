using NewsMixer.Models;
using System.Runtime.CompilerServices;

namespace NewsMixer.Transforms.DummySummary
{
    public class DummySummaryTransform : ITransform
    {
        public async IAsyncEnumerable<NewsItem> Execute(IAsyncEnumerable<NewsItem> items, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach(var itm in items)
            {
                itm.Content = itm.Content?[..200];
                yield return itm;
            }
        }
    }
}
