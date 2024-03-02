using NewsMixer;
using NewsMixer.InputSources.DummySource;
using NewsMixer.Output.Console;
using NewsMixer.Output.RssFile;
using NewsMixer.Transforms.OpenAiSummary;

Console.WriteLine("This is the NewsMixer!");
Console.WriteLine("Error 451 Unavailable for Legal Reasons");
Console.WriteLine("");

var source = new DummySourceInput();
var apiKey = System.Environment.GetEnvironmentVariable("OPENAI_APIKEY") ?? string.Empty;
var outputFolder = "c:\\temp";
var baseUrl = "https://sitecore-hackathon.github.io/2024-Team-451-Unavailable-For-Legal-Reasons";

var pipeline = new Pipeline().AddInput(source)
    .AddStream(cfg =>
    {
        cfg.AddTransform(
        new OpenAiSummaryTransform(new OpenAiSummaryConfiguration()
        {
            ApiKey = apiKey,
            AiBehavior = "You are a romantic poet that only uses up to four syllabuses in each word",
            Language = "English",
        })
        );
        cfg.AddOutput(
            new RssFileOutput(new RssFileConfiguration
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
            new RssFileOutput(new RssFileConfiguration
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
        new OpenAiSummaryTransform(new OpenAiSummaryConfiguration()
        {
            ApiKey = apiKey,
            AiBehavior = "You are a gossipy news editor who likes to use click-baits and have a victorian writing style",
            Language = "da-DK"
        })
        );
        cfg.AddOutput(
            new RssFileOutput(new RssFileConfiguration
            {
                Digest = DigestFormat.Daily,
                FileFormatPattern = "gossipy-daily-{date}-da-DK.rss",
                OutputFolder = outputFolder,
                FeedTitle = "Your editor's daily update",
                FeedDescription = "Summaries from an editor",
                FeedId = "editor-daily",
                FeedUrl = new Uri($"{baseUrl}/editor-daily.rss"),
                SiteUrl = new Uri($"{baseUrl}")
            }),
            new RssFileOutput(new RssFileConfiguration
            {
                Digest = DigestFormat.Weekly,
                FileFormatPattern = "gossipy-weekly-{date}-da-DK.rss",
                OutputFolder = outputFolder,
                FeedTitle = "Your editor's weekely update",
                FeedDescription = "Summaries from an editor",
                FeedId = "editor-daily",
                FeedUrl = new Uri($"{baseUrl}/editor-daily.rss"),
                SiteUrl = new Uri($"{baseUrl}")
            }),
            new ConsoleOutput("[Gossipy] ")
        );
    });

await pipeline.Execute(new CancellationToken());
