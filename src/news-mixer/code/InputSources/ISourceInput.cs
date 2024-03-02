using NewsMixer.Models;

namespace NewsMixer.InputSources
{
    internal interface ISourceInput
    {
        public IAsyncEnumerable<NewsItem> Execute(CancellationToken token);
    }
}
