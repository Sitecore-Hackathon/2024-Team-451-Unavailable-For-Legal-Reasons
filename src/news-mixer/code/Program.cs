using GraphQL.Client.Abstractions.Websocket;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewsMixer;
using NewsMixer.InputSources.SitecoreGraph;
using NewsMixer.Output.Console;
using NewsMixer.Output.RssFile;
using NewsMixer.Transforms.OpenAiSummary;

var pipeline = new Pipeline();
var services = new ServiceCollection();

services.AddHttpClient()
        .AddSingleton<AuthenticationTokenService>()
        .AddSingleton<IGraphQLWebsocketJsonSerializer>(new SystemTextJsonSerializer())
        .AddSingleton<GraphQlClientFactory>()
        .AddSingleton(LoggerFactory.Create(builder => builder.AddConsole().AddFilter(typeof(HttpClient).Namespace, LogLevel.Warning)))
        .AddSingleton(x => pipeline);

var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

Console.WriteLine("### This is the NewsMixer! ###");
Console.WriteLine("### By Team Error 451 Unavailable for Legal Reasons ###");
Console.WriteLine("");

logger.LogInformation("starting...");

// setup pipeline
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

var source = new SitecoreGraphInputSource(config, serviceProvider.GetRequiredService<GraphQlClientFactory>());
var apiKey = Environment.GetEnvironmentVariable("OPENAI_APIKEY") ?? throw new ArgumentException("OPENAI_APIKEY environment variable is missing.");
var outputFolder = Environment.GetEnvironmentVariable("OUTPUT_DIR") ?? Environment.GetEnvironmentVariable("TEMP") ?? throw new ArgumentException("OUTPUT_DIR or TEMP environment variable is missing.");
var baseUrl = Environment.GetEnvironmentVariable("FEED_BASEURL") ?? "https://sitecore-hackathon.github.io/2024-Team-451-Unavailable-For-Legal-Reasons";

pipeline.AddInput(source)
    .AddStream(cfg =>
    {
        cfg.AddTransform(
            new OpenAiTitleTranslation(new()
            {
                ApiKey = apiKey,
                Language = "en",
            })
            ,
            new OpenAiSummaryTransform(new()
            {
                ApiKey = apiKey,
                AiBehavior = "You are a romantic poet that only uses up to four syllabuses in each word",
                Language = "en",
            })
        );

        cfg.AddOutput(
            new RssFileOutput(new()
            {
                Digest = DigestFormat.Daily,
                FileFormatPattern = "poet-daily-{date}.rss",
                OutputFolder = outputFolder,
                FeedTitle = "Your poet's daily update",
                FeedDescription = "Summaries from a romantic poet",
                FeedId = "poet-daily",
                FeedUrl = new Uri($"{baseUrl}/poet-daily.rss"),
                SiteUrl = new Uri($"{baseUrl}")
            }),
            new RssFileOutput(new()
            {
                Digest = DigestFormat.Weekly,
                FileFormatPattern = "poet-weekly-{date}.rss",
                OutputFolder = outputFolder,
                FeedTitle = "Your poet's weekly update",
                FeedDescription = "Summaries from a romantic poet",
                FeedId = "poet-weekly",
                FeedUrl = new Uri($"{baseUrl}/poet-weekly.rss"),
                SiteUrl = new Uri($"{baseUrl}")
            }),
            new ConsoleOutput("[Poet en]")
        );
    })
    .AddStream(cfg =>
    {
        cfg.AddTransform(
            new OpenAiTitleTranslation(new()
            {
                ApiKey = apiKey,
                Language = "en",
            }),
            new OpenAiSummaryTransform(new OpenAiSummaryConfiguration()
            {
                ApiKey = apiKey,
                AiBehavior = "You are a gossipy news editor who likes to use click-baits and have a victorian writing style",
                Language = "en"
            })
        );
        cfg.AddOutput(
            new RssFileOutput(new RssFileConfiguration
            {
                Digest = DigestFormat.Daily,
                FileFormatPattern = "gossipy-daily-{date}-en.rss",
                OutputFolder = outputFolder,
                FeedTitle = "Your editor's daily update",
                FeedDescription = "Summaries from an editor",
                FeedId = "gossipy-editor-daily",
                FeedUrl = new Uri($"{baseUrl}/editor-daily.rss"),
                SiteUrl = new Uri($"{baseUrl}")
            }),
            new RssFileOutput(new RssFileConfiguration
            {
                Digest = DigestFormat.Weekly,
                FileFormatPattern = "gossipy-weekly-{date}-en.rss",
                OutputFolder = outputFolder,
                FeedTitle = "Your editor's weekely update",
                FeedDescription = "Summaries from an editor",
                FeedId = "gossipy-editor-daily",
                FeedUrl = new Uri($"{baseUrl}/editor-daily.rss"),
                SiteUrl = new Uri($"{baseUrl}")
            }),
            new ConsoleOutput("[Gossipy en]")
        );
    })
    .AddStream(cfg =>
    {
        cfg.AddTransform(
            new OpenAiTitleTranslation(new()
            {
                ApiKey = apiKey,
                Language = "da-DK",
            }),
            new OpenAiSummaryTransform(new()
            {
                ApiKey = apiKey,
                AiBehavior = "You are a romantic poet that only uses up to four syllabuses in each word",
                Language = "da-DK",
            })
        );
        cfg.AddOutput(
            new RssFileOutput(new()
            {
                Digest = DigestFormat.Weekly,
                FileFormatPattern = "poet-weekly-{date}-da.rss",
                OutputFolder = outputFolder,
                FeedTitle = "Dine lyriske updates",
                FeedDescription = "Summaries from a romantic poet",
                FeedId = "poet-daily-da",
                FeedUrl = new Uri($"{baseUrl}/poet-weekly-da.rss"),
                SiteUrl = new Uri($"{baseUrl}")
            }),
            new ConsoleOutput("[Poet da]")
        );
    });

// handle ctrl+c grcefully
var cts = new CancellationTokenSource();

Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Canceling...");

    cts.Cancel();

    e.Cancel = true;
};

// start
await pipeline.Execute(cts.Token);

logger.LogInformation("finished.");
