namespace NewsMixer.Output.RssFile
{
    public class RssFileConfiguration
    {
        public string OutputFolder { get; set; }

        public string FileFormatPattern { get; set; }

        public DigestFormat Digest { get; set; } 
    }

    public enum DigestFormat
    {
        Daily,
        Weekly
    }
}