using NewsMixer.Extensions;
using NewsMixer.Models;
using System.Runtime.CompilerServices;

namespace NewsMixer.InputSources.RandomSource
{
    public class RandomizeInputSource : ISourceInput
    {
        private readonly ISourceInput _innerSource;
        private readonly int _maxItems;

        public RandomizeInputSource(int maxItems, ISourceInput innerSource)
        {
            _innerSource = innerSource;
            _maxItems = maxItems;
        }

        public async IAsyncEnumerable<NewsItem> Execute([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var enumerable = _innerSource.Execute(cancellationToken);
            var list = await enumerable.ToListAsync();
            foreach (var itm in list.OrderBy(x => Guid.NewGuid()).Take(_maxItems))
            {
                yield return itm;
            }
        }
    }
}
