namespace NewsMixer.Models
{
    public class NewsItem
    {
        public ICollection<string>? Categories { get; set; }
        public string OriginalLanguage { get; set; } = null!;
        public string? ContentLanguage { get;set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Url { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? ImageUrl { get; set; }
    }
}
