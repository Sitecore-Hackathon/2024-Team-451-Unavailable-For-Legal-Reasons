using NewsMixer.InputSources.SitecoreGraph;

namespace NewsMixer.UnitTests.InputSources.SitecoreGraph
{
    public class SitecoreTemplatesGraphConfigurationShould
    {
        [Fact]
        public void CreateQueryWithInterpolation()
        {
            var config = new SitecoreTemplatesGraphConfiguration
            {
                ContentField = "contentfieldname",
                TitleField = "titlefieldname",
                Language = "sk",
                TemplateId = new Guid("A0000000-0000-0000-0000-000000000001"),
                RootItemId = new Guid("B0000000-0000-0000-0000-000000000002"),
            };

            var actual = config.Query;

            Assert.Contains("title: titlefieldname { value }", actual);
            Assert.Contains("content: contentfieldname { value }", actual);
            Assert.Contains("{ name: \"_language\",  value: \"sk\" },", actual);
        }
    }
}
