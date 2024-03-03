using System.Collections.Concurrent;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace NewsMixer.InputSources.SitecoreGraph;

public class AuthenticationTokenService(IHttpClientFactory httpClientFactory)
{
    private readonly ConcurrentDictionary<Uri, BearerToken> _tokens = new();
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<BearerToken> GetTokenAsync(EndPointConfiguration endpoint, CancellationToken cancellationToken)
    {
        if (_tokens.TryGetValue(endpoint.AuthenticationTokenUri, out var token))
        {
            if (token.IsValid())
            {
                return token;
            }
        }
                
        using var httpClient = _httpClientFactory.CreateClient();
        using var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = endpoint.AuthenticationTokenUri,
            Content = new FormUrlEncodedContent(endpoint.GetClientCredentials()),
        };

        using var response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {

            throw new AuthenticationTokenException($"Failure during authentication request.");
        }

        using var jsonStream = await response.Content.ReadAsStreamAsync();
        var authResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<AuthResponse>(jsonStream, cancellationToken: cancellationToken);

        if (authResponse == null)
        {

            throw new AuthenticationTokenException("Could not deserialize auth response.");
        }

        token = new BearerToken(authResponse.access_token, DateTime.UtcNow.AddSeconds(authResponse.expires_in));

        _tokens[endpoint.AuthenticationTokenUri] = token;

        return token;
    }

    private record AuthResponse(int expires_in)
    {
        public string access_token { get; set; } = null!;
    }
}

public class BearerToken
{
    public string Token { get; }

    private readonly DateTime _expiresUtc;

    public BearerToken(string token, DateTime expiresUtc)
    {
        Token = token;

        _expiresUtc = expiresUtc;
    }

    public bool IsValid()
    {
        return DateTime.UtcNow < _expiresUtc;
    }
}
