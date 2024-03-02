namespace NewsMixer.InputSources.SitecoreGraph
{
    public abstract class SitecoreGraphInputConfiguration
    {
        public EndPointConfiguration EndPoint { get; set; } = null!;

        public abstract string GetQuery();

        public abstract dynamic GetVariables(string? cursor);
    }
}