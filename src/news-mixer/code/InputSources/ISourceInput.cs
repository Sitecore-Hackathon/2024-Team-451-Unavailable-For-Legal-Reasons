using Microsoft.Extensions.Logging;
using NewsMixer.Models;

namespace NewsMixer.InputSources
{
    public interface ISourceInput
    {
        public IAsyncEnumerable<NewsItem> Execute(CancellationToken cancellationToken);
    }
}
