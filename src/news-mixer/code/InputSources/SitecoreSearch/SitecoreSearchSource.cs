using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewsMixer.Models;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace NewsMixer.InputSources.SitecoreSearch
{

    public class SitecoreSearchSource : ISourceInput
    {
        private readonly SitecoreSearchConfiguration _config;
        private readonly HttpClient _client;
        private readonly ILogger? _logger;

        public SitecoreSearchSource(SitecoreSearchConfiguration config, IServiceProvider sp) : this(config, sp.GetRequiredService<ILogger<SitecoreSearchSource>>(), sp.GetRequiredService<IHttpClientFactory>().CreateClient())
        {
        }
        public SitecoreSearchSource(SitecoreSearchConfiguration config, ILogger logger, HttpClient client)
        {
            _config = config;
            _client = client;
            _client.DefaultRequestHeaders.Add("Authorization", config.SearchApiKey);
            _logger = logger;
        }

        public IAsyncEnumerable<NewsItem> Execute(CancellationToken cancellationToken)
        {
            var source = FetchSearchResults(cancellationToken);
            var result = FetchPagesContent(source, cancellationToken);
            return result;
        }

        private async IAsyncEnumerable<NewsItem> FetchSearchResults([EnumeratorCancellation]CancellationToken cancellationToken)
        {
            var searchEndpointUri = $"{_config.SearchEndpoint}/{_config.SearchDomainId}";
            var response = await _client.PostAsJsonAsync(searchEndpointUri, _config.SearchQueryObject, cancellationToken);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<SearchResponse>(cancellationToken);
            var items = (data?.Widgets?.First()?.Content) ?? throw new ArgumentException("Unexpected search result");
            var language = $"{_config.ContextLanguage}-{_config.ContextCountry}";

            foreach (var itm in items)
            {
                yield return new NewsItem
                {
                    Title = itm.Name,
                    Content = itm.Description,
                    Url = itm.Url,
                    OriginalLanguage = language,
                    ContentLanguage = language,
                };
            }
        }

        public async IAsyncEnumerable<NewsItem> FetchPagesContent(IAsyncEnumerable<NewsItem> source, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach(var item in source)
            {
                yield return await FetchPageContent(item, cancellationToken);
            }
        }

        private async Task<NewsItem> FetchPageContent(NewsItem item, CancellationToken cancellationToken)
        {
            if (!_config.FetchPageContent)
            {
                return item;
            }

            var url = item.Url.ToString();
            var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            _logger?.LogDebug("Fetch page {PageUrl}", url);
            var document = await context.OpenAsync(url, cancellationToken);
            _logger?.LogDebug("Find content on {PageUrl}", url);
            var article = document.QuerySelector("article");
            var content = article?.TextContent?.Trim();
            if (!string.IsNullOrEmpty(content) && ((content?.Length ?? 0) > (item.Content?.Length ?? 0)))
            {
                item.Content = content!;
            }

            return item;
        }
    }
}
