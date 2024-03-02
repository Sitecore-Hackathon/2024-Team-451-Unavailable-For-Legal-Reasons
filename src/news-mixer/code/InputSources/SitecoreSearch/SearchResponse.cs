namespace NewsMixer.InputSources.SitecoreSearch
{
    public class SearchResponse
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SearchWidgetResponse[] Widgets { get; set; }

    }
    public class SearchWidgetResponse
    {
        public SearchContentItem[] Content { get; set; }
    }

    public class SearchContentItem
    {
        public Uri Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
