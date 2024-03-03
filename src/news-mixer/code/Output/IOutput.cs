using NewsMixer.Models;

namespace NewsMixer.Output
{
    public interface IOutput
    {
        public Task Execute(NewsItem items, CancellationToken cancellationToken = default);
    }
}
