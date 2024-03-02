﻿namespace NewsMixer.Models
{
    public class NewsItem
    {
        public ICollection<string>? Categories { get; set; }
        public string OriginalLanguage { get; set; }
        public string ContentLanguage { get;set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Url { get; set; }
        public DateTime? Date { get; set; }
        public string ImageUrl { get; set; }
    }
}
