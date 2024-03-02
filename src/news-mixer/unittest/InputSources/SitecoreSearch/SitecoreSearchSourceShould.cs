using NewsMixer.InputSources.SitecoreSearch;
using NewsMixer.Models;
using NewsMixer.UnitTests.Extensions;

namespace NewsMixer.UnitTests.InputSources.SitecoreSearch
{
    public class SitecoreSearchSourceShould
    {
        [Fact]
        public async Task InvokeSearchService()
        {
            var sut = new SitecoreSearchSource(new()
            {
                FetchPageContent = false,
                Limit = 2,
                QueryPhrase = "GraphQL"
            });

            var actual = sut.Execute(CancellationToken.None);

            Assert.All(actual, x =>
            {
                Assert.NotNull(x.Url);
                Assert.NotNull(x.Title);
            });
        }

        [Fact]
        public async Task FetchPage()
        {
            var sut = new SitecoreSearchSource(new()
            {
                FetchPageContent = true,
                Limit = 2,
                QueryPhrase = "GraphQL"
            });

            var enumerable = new[] {new NewsItem
            {
                Url = new Uri("https://doc.sitecore.com/xp/en/developers/hd/21/sitecore-headless-development/graphql.html")
            } };
            var result = sut.FetchPagesContent(enumerable.ToAsyncEnumerable(), CancellationToken.None);
            var actual = await result.ToListAsync();

            Assert.Single(actual);
            Assert.NotNull(actual[0].Content);
        }
    }
}
