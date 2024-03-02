using NewsMixer;
using NewsMixer.InputSources.DummySource;
using NewsMixer.Output.Console;
using NewsMixer.Output.RssFile;
using NewsMixer.Transforms.OpenAiSummary;

Console.WriteLine("This is the NewsMixer!");
Console.WriteLine("Error 451 Unavailable for Legal Reasons");
Console.WriteLine("");

var source = new DummySourceInput();
var apiKey = System.Environment.GetEnvironmentVariable("OPENAI_APIKEY") ?? throw new ArgumentException("OPENAI_APIKEY environment variable is missing.");
var outputFolder = System.Environment.GetEnvironmentVariable("OUTPUT_DIR") ?? "c:\\temp";
var baseUrl = System.Environment.GetEnvironmentVariable("FEED_BASEURL") ?? "https://sitecore-hackathon.github.io/2024-Team-451-Unavailable-For-Legal-Reasons";

// Setup pipeline
var pipeline = new Pipeline().AddInput(source)
    .AddStream(cfg =>
    {
        cfg.AddTransform(
            new OpenAiTitleTranslation(new()
            {
                ApiKey = apiKey,
                Language = "en",
            }),
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
            new ConsoleOutput("[Poet] ")
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
            new ConsoleOutput("[Gossipy] ")
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
            new ConsoleOutput("[Poet] ")
            );
    });

// Handle Ctrl+C grcefully
var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Canceling...");
    cts.Cancel();
    e.Cancel = true;
};

// Start running
await pipeline.Execute(cts.Token);
