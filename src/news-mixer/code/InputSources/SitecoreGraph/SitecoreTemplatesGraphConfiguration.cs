namespace NewsMixer.InputSources.SitecoreGraph
{
    public class SitecoreTemplatesGraphConfiguration : SitecoreGraphInputConfiguration
    {
        public Guid RootItemId { get; set; }

        public Guid TemplateId { get; set; }

        public string? Language { get; set; }

        public string? TitleField { get; set; }

        public string? ContentField { get; set; }

        public new string Query
        {
            get
            {
                return GetQueryTemplate()
                    .Replace("{rootItemId}", RootItemId.ToString("N"))
                    .Replace("{templateId}", TemplateId.ToString("N"))
                    .Replace("{language}", Language)
                    .Replace("{titleField}", TitleField)
                    .Replace("{contentField}", ContentField);
            }
            set { }
        }

        private static string? _template = null;

        private static string GetQueryTemplate()
        {
            if (_template != null)
            {
                return _template;
            }

            var t = typeof(SitecoreTemplatesGraphConfiguration);
            var resourceName = t.FullName?.Replace(t.Name, "SitecoreTemplatesGraph.graphql") ?? throw new InvalidOperationException("Type is missing FullName, consider compilation flow");
            using var stream = typeof(SitecoreTemplatesGraphConfiguration).Assembly.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);
            _template = reader.ReadToEnd();
            return _template;
        }
    }
}