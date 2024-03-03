using GraphQL.Client.Abstractions.Websocket;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using NewsMixer.InputSources.SitecoreGraph;
using Xunit.Abstractions;

namespace NewsMixer.UnitTests.InputSources.SitecoreGraph
{
    public class SitecoreTemplatesGraphConfigurationShould
    {
        private readonly ITestOutputHelper _output;

        public SitecoreTemplatesGraphConfigurationShould(ITestOutputHelper output) => _output = output;

        [Fact]
        public void GetQueryVariables()
        {
            var config = new SitecoreTemplatesGraphConfiguration
            {
                ContentField = "contentfieldname",
                TitleField = "titlefieldname",
                Language = "en",
                TemplateIds = [
                    new Guid("FF095022-530E-46AC-BC22-816A11C3A1BD"),
                    new Guid("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
                    ],
                RootItemId = new Guid("110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9"),
            };

            var cursor = "NQ==";

            var actual = config.GetVariables(cursor);

            Assert.Equal(cursor, actual.cursor);
            Assert.Equal(config.RootItemId, actual.rootId);
            Assert.Equal(config.ContentField, actual.contentField);
            Assert.Equal(config.TitleField, actual.titleField);
            Assert.Equal(config.Language, actual.language);
            Assert.Equal(config.TemplateIds.Skip(0).First(), actual.includeTemplate1);

            Assert.Equal(config.TemplateIds.Skip(1).First(), actual.includeTemplate2);
            Assert.Equal(Guid.Empty, actual.includeTemplate3);
            Assert.Equal(Guid.Empty, actual.includeTemplate4);
            Assert.Equal(Guid.Empty, actual.includeTemplate5);
            Assert.Equal(Guid.Empty, actual.includeTemplate6);
            Assert.Equal(Guid.Empty, actual.includeTemplate7);
            Assert.Equal(Guid.Empty, actual.includeTemplate8);
            Assert.Equal(Guid.Empty, actual.includeTemplate9);
            Assert.Equal(Guid.Empty, actual.includeTemplate10);
        }

        [Fact]
        public async Task Execute()
        {
            var config = new SitecoreTemplatesGraphConfiguration
            {
                EndPoint = new UsernameAndPasswordEndPointConfiguration
                {
                    ApiUri = new Uri("https://cm.team451.localhost/sitecore/api/graph/edge/"),
                    ApiKey = "{F0107448-59B2-40D0-9158-B6F33F17E9C4}",
                    AuthenticationTokenUri = new Uri("https://id.team451.localhost/connect/token"),
                    ClientId = "newsmixer",
                    Username = "sitecore\\admin",
                    Password = "b"
                },
                ContentField = "body",
                TitleField = "title",
                Language = "en",
                TemplateIds = [
                    new Guid("FF095022-530E-46AC-BC22-816A11C3A1BD"),
                    new Guid("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
                    ],
                RootItemId = new Guid("110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9"),
            };

            var services = new ServiceCollection()
                .AddHttpClient()
                .AddSingleton<AuthenticationTokenService>()
                .AddSingleton<IGraphQLWebsocketJsonSerializer>(new SystemTextJsonSerializer())
                .AddSingleton<GraphQlClientFactory>();

            var sp = services.BuildServiceProvider();
            var source = new SitecoreGraphInputSource(config, sp.GetRequiredService<GraphQlClientFactory>());
            var count = 0;

            await foreach (var item in source.Execute(CancellationToken.None))
            {
                _output.WriteLine(item.Title[..30] + "=" + item.Url.ToString());

                Assert.NotNull(item.Title);
                Assert.NotNull(item.Content);
                Assert.NotNull(item.Url);

                Interlocked.Increment(ref count);
            }

            Assert.Equal(50, count);
        }
    }
}
