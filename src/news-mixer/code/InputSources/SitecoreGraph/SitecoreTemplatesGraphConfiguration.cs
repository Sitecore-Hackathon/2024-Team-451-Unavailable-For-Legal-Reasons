namespace NewsMixer.InputSources.SitecoreGraph;

public class SitecoreTemplatesGraphConfiguration : SitecoreGraphInputConfiguration
{
    public Guid RootItemId { get; set; }

    public required Guid[] TemplateIds { get; set; }

    public string Language { get; set; } = "en";

    public string TitleField { get; set; } = "title";

    public string ContentField { get; set; } = "content";

    private static string? _template;

    public override string GetQuery()
    {
        if (_template != null)
        {
            return _template;
        }

        var type = typeof(SitecoreTemplatesGraphConfiguration);

        ArgumentException.ThrowIfNullOrEmpty(type.FullName);

        var resourceName = type.FullName.Replace(type.Name, "SitecoreTemplatesGraph.graphql");

        using var stream = typeof(SitecoreTemplatesGraphConfiguration).Assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);

        _template = reader.ReadToEnd();

        return _template;
    }

    public override dynamic GetVariables(string? cusor)
    {
        // NOTE: yeah this is fckd up, blame Sitecore :-P
        var includeTemplate1 = TemplateIds.First();
        var includeTemplate2 = TemplateIds.Skip(1).FirstOrDefault();
        var includeTemplate3 = TemplateIds.Skip(2).FirstOrDefault();
        var includeTemplate4 = TemplateIds.Skip(3).FirstOrDefault();
        var includeTemplate5 = TemplateIds.Skip(4).FirstOrDefault();
        var includeTemplate6 = TemplateIds.Skip(5).FirstOrDefault();
        var includeTemplate7 = TemplateIds.Skip(6).FirstOrDefault();
        var includeTemplate8 = TemplateIds.Skip(7).FirstOrDefault();
        var includeTemplate9 = TemplateIds.Skip(8).FirstOrDefault();
        var includeTemplate10 = TemplateIds.Skip(9).FirstOrDefault();

        return new SitecoreGetContentVariables(cusor, RootItemId, TitleField, ContentField, includeTemplate1
            , includeTemplate2
            , includeTemplate3
            , includeTemplate4
            , includeTemplate5
            , includeTemplate6
            , includeTemplate7
            , includeTemplate8
            , includeTemplate9
            , includeTemplate10
            , Language);
    }
}

public record SitecoreGetContentVariables(string? cursor, Guid rootId, string titleField, string contentField, Guid includeTemplate1
    , Guid includeTemplate2
    , Guid includeTemplate3
    , Guid includeTemplate4
    , Guid includeTemplate5
    , Guid includeTemplate6
    , Guid includeTemplate7
    , Guid includeTemplate8
    , Guid includeTemplate9
    , Guid includeTemplate10
    , string? language);
