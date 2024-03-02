using NewsMixer.Models;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsMixer.Output.RssFile
{
    public class RssFileOutput : IOutput
    {
        private readonly RssFileConfiguration _config;
        private readonly object _lock = new object();

        public RssFileOutput(RssFileConfiguration config)
        {
            _config = config;
            config.EnsureOutputFolder();
        }

        public Task Execute(NewsItem itm, CancellationToken cancellationToken = default)
        {
            var filePath = _config.GetFilePath(itm.Date ?? DateTimeOffset.Now);

            lock (_lock)
            {
                var feed = GetExistingFeed(filePath) ?? CreateFeed();

                var syndicationItem = new SyndicationItem
                {
                    PublishDate = itm.Date ?? DateTimeOffset.Now,
                    Title = new TextSyndicationContent(itm.Title, TextSyndicationContentKind.Plaintext),
                    Summary = new TextSyndicationContent($"[{itm.OriginalLanguage}] " + itm.Content, TextSyndicationContentKind.Plaintext),
                };
                syndicationItem.Links.Add(new SyndicationLink(itm.Url));

                feed.Items = (feed.Items ?? [])
                    .Union([syndicationItem]);

                return WriteFeed(feed, filePath);
            }
        }

        private static SyndicationFeed? GetExistingFeed(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using var stream = File.OpenRead(filePath);
            using var xmlReader = XmlReader.Create(stream);
            var feed = SyndicationFeed.Load(xmlReader);
            return feed;
        }

        private SyndicationFeed CreateFeed()
        {
            return new SyndicationFeed(_config.FeedTitle, _config.FeedDescription, _config.FeedUrl)
            {
                Generator = "Team 451 News Mixer",
                BaseUri = _config.FeedUrl,
                Id = _config.FeedId,
            };
        }

        private static Task WriteFeed(SyndicationFeed feed, string filePath)
        {
            var settings = new XmlWriterSettings
            {
                Encoding = System.Text.Encoding.UTF8,
                Indent = true,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
            };

            using var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            using var writer = XmlWriter.Create(stream, settings);
            var formatter = feed.GetRss20Formatter();
            formatter.WriteTo(writer);
            return Task.CompletedTask;
        }
    }
}
