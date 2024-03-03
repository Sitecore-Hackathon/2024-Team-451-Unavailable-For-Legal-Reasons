using System.Globalization;

namespace NewsMixer.Output.RssFile
{
    public class RssFileConfiguration
    {
        public string OutputFolder { get; set; } = Path.GetTempPath();

        public string FileFormatPattern { get; set; } = "{date}.rss";

        public DigestFormat Digest { get; set; }

        public string FeedTitle { get; set; } = "Title";
        public string FeedDescription { get; set; } = "";
        public Uri SiteUrl { get; set; } = new("https://valtech.com");
        public Uri FeedUrl { get; set; } = new("https://valtech.com");
        public string FeedId { get; set; } = "RssId";

        public void EnsureOutputFolder()
        {
            if (!Directory.Exists(OutputFolder)) { Directory.CreateDirectory(OutputFolder); }
        }

        public string GetFilePath(DateTimeOffset date)
        {
            var dateStr = Digest switch
            {
                DigestFormat.Daily => date.ToString("yyyy-MM-dd"),
                DigestFormat.Weekly => $"week-{CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday):D2}",
                _ => "date",
            };
            return Path.Join(OutputFolder, FileFormatPattern.Replace("{date}", dateStr));
        }
    }

    public enum DigestFormat
    {
        Daily,
        Weekly
    }
}