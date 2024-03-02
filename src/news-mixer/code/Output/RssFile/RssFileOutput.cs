using NewsMixer.Models;

namespace NewsMixer.Output.RssFile
{
    public class RssFileOutput : IOutput
    {
        private readonly RssFileConfiguration _config;

        public RssFileOutput(RssFileConfiguration config)
        {
            _config = config;
        }

        public Task Execute(NewsItem itm, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
