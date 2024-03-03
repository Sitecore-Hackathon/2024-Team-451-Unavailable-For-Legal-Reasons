using AngleSharp.Dom;
using GraphQL;
using NewsMixer.Models;
using System.Runtime.CompilerServices;

namespace NewsMixer.InputSources.SitecoreGraph
{
    public class SitecoreGraphInputSource : ISourceInput
    {
        private readonly SitecoreTemplatesGraphConfiguration _config;
        private readonly GraphQlClientFactory _graphQlClientFactory;

        public SitecoreGraphInputSource(SitecoreGraphInputConfiguration config, GraphQlClientFactory graphQlClientFactory)
        {
            _config = (SitecoreTemplatesGraphConfiguration)config;
            _graphQlClientFactory = graphQlClientFactory;
        }

        public async IAsyncEnumerable<NewsItem> Execute([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var client = await _graphQlClientFactory.CreateClientAsync(_config.EndPoint, cancellationToken);
            GraphQLResponse<GetContentResponseDto> response = null!;

            do
            {
                response = await client.SendQueryAsync<GetContentResponseDto>(new GraphQLRequest(_config.GetQuery(), _config.GetVariables(response?.Data.Search.PageInfo.EndCursor)), cancellationToken);

                foreach (var item in response.Data.Search.Results)
                {
                    yield return new NewsItem
                    {
                        Title = item.Title.Value,
                        Content = item.Content.Value,
                        Date = DateTime.UtcNow,
                        OriginalLanguage = _config.Language,
                        Categories = ["woop", "test"],
                        Url = new Uri($"{item.Url.Scheme}://{item.Url.Hostname}{item.Url.Path}")
                    };
                }
            } while (response.Data.Search.PageInfo.HasNext);
        }
    }

    public record GetContentResponseDto(SearchDto Search);

    public record SearchDto(int Total, PageInfoDto PageInfo, ResultDto[] Results);

    public record PageInfoDto(string EndCursor, bool HasNext);

    public record ResultDto(FieldDto Title, FieldDto Content, UrlFieldDto Url);

    public record FieldDto(string Value);

    public record UrlFieldDto(string Path, string Hostname, string Scheme);
}
