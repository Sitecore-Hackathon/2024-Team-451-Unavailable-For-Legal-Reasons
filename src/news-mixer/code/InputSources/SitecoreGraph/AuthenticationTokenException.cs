using System.Runtime.Serialization;

namespace NewsMixer.InputSources.SitecoreGraph;

[Serializable]
public class AuthenticationTokenException : Exception
{
    public AuthenticationTokenException(string message) : base(message)
    {

    }

    protected AuthenticationTokenException(SerializationInfo info, StreamingContext ctx)
    {

    }
}
