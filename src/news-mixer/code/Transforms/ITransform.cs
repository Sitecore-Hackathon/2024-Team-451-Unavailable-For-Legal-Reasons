using Microsoft.Extensions.Logging;
using NewsMixer.Models;

namespace NewsMixer.Transforms
{
    public interface ITransform
    {
        public Task<NewsItem> Execute(NewsItem items, ILogger logger, CancellationToken cancellationToken);
    }
}
