
namespace NewsMixer.InputSources.SitecoreGraph;

public class EndPointConfiguration
{
    private Dictionary<string, string>? _clientCredentials;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public Uri ApiUri { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public Uri AuthenticationTokenUri { get; set; } = null!;

    public virtual Dictionary<string, string> GetClientCredentials()
    {
        _clientCredentials ??= new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
            };

        return _clientCredentials;
    }
}

public class UsernameAndPasswordEndPointConfiguration : EndPointConfiguration
{
    private Dictionary<string, string>? _clientCredentials;

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public override Dictionary<string, string> GetClientCredentials()
    {
        _clientCredentials ??= new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", ClientId },
                { "username", Username },
                { "password", Password }
            };

        return _clientCredentials;
    }
}
