using NewsMixer.Models;
using System.Runtime.CompilerServices;

namespace NewsMixer.InputSources.SitecoreSearch
{
    public class SitecoreSearchConfiguration
    {
        public Uri SearchEndpoint { get; set; } = new Uri("https://discover.sitecorecloud.io/discover/v2/");

        public string? SearchDomainId { get; set; } = "43533744";

        public string? SearchApiKey { get; set; } = "01-5370450c-b5ef9b415170c314ee8bc9c99d4f51410f601cb8";

        public string ContextLocale { get; set; } = "en-us";

        public string ContextPage { get; set; } = "/search-results";

        public string WidgetRfkId { get; set; } = "rfkid_7";

        public string Entity { get; set; } = "content";

        public string? QueryPhrase { get; set; }

        public string Sort { get; set; } = "featured";

        public string[] Sources { get; set; } = [];

        public bool FetchPageContent { get; set; } = true;

    }

    public class SitecoreSearchSource : ISourceInput
    {
        public async IAsyncEnumerable<NewsItem> Execute([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            yield break;
        }
    }
}
