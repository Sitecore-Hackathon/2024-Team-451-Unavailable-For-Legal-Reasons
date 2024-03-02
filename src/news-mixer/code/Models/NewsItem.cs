namespace NewsMixer.Models
{
    public class NewsItem
    {
        public ICollection<string>? Categories { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? ContentLanguage { get;set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Uri? Url { get; set; }
        public DateTimeOffset? Date { get; set; }
        public Uri? ImageUrl { get; set; }
    }
}
