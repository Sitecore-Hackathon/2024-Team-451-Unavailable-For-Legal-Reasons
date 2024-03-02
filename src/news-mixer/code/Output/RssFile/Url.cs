using System.Diagnostics.CodeAnalysis;

namespace NewsMixer.Output.RssFile
{
    internal class Url : Uri
    {
        public Url([StringSyntax("Uri")] string uriString) : base(uriString)
        {
        }
    }
}