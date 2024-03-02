namespace NewsMixer.Output.RssFile
{
    public class RssFileConfiguration
    {
        public string OutputFolder { get; set; } = null!;

        public string FileFormatPattern { get; set; } = null!;

        public DigestFormat Digest { get; set; } 
    }

    public enum DigestFormat
    {
        Daily,
        Weekly
    }
}