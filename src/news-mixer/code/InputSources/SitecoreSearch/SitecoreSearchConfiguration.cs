namespace NewsMixer.InputSources.SitecoreSearch
{
    public class SitecoreSearchConfiguration
    {
        public Uri SearchEndpoint { get; set; } = new Uri("https://discover.sitecorecloud.io/discover/v2");

        public string? SearchDomainId { get; set; } = "43533744";

        public string? SearchApiKey { get; set; } = "01-5370450c-b5ef9b415170c314ee8bc9c99d4f51410f601cb8";

        public string ContextLanguage { get; set; } = "en";
        public string ContextCountry { get; set; } = "us";

        public string ContextPage { get; set; } = "/search-results";

        public string WidgetRfkId { get; set; } = "rfkid_7";

        public string Entity { get; set; } = "content";

        public string? QueryPhrase { get; set; }

        public string Sort { get; set; } = "featured";

        public string[] Sources { get; set; } = [];

        public int Limit { get; set; } = 20;

        public bool FetchPageContent { get; set; } = true;

        public object SearchQueryObject => new
        {
            context = new
            {
                locale = new
                {
                    language = ContextLanguage.ToLowerInvariant(),
                    country = ContextCountry.ToLowerInvariant(),
                },
                page = new { uri = ContextPage.ToString() },
            },
            widget = new
            {
                items = new[]{
                    new {
                        rfk_id = WidgetRfkId,
                        entity = Entity,
                        search = new
                        {
                            content = new { },
                            query = new
                            {
                                keyphrase = QueryPhrase
                            },
                            limit = Limit,
                            facet = new
                            {
                                all = false
                            },
                            sort = new {
                                choices=false,
                                value = new[]
                                {
                                    new { name = Sort }
                                }
                            },
                        },
                        sources = Sources?.Length != 0 ? Sources : null
                    },
                }
            }
        };
    }
}
