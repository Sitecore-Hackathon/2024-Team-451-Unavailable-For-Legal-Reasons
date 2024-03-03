using GraphQL.Client.Abstractions.Websocket;
using GraphQL.Client.Http;
using System.Net.Http.Headers;

namespace NewsMixer.InputSources.SitecoreGraph;

public class GraphQlClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthenticationTokenService _authenticationTokenService;
    private readonly IGraphQLWebsocketJsonSerializer _serializer;

    public GraphQlClientFactory(IHttpClientFactory httpClientFactory, AuthenticationTokenService authenticationTokenService, IGraphQLWebsocketJsonSerializer serializer)
    {
        _httpClientFactory = httpClientFactory;
        _authenticationTokenService = authenticationTokenService;
        _serializer = serializer;
    }

    public async Task<GraphQLHttpClient> CreateClientAsync(EndPointConfiguration endpoint, CancellationToken cancellationToken)
    {
        var accessToken = await _authenticationTokenService.GetTokenAsync(endpoint, cancellationToken);

        var httpClient = _httpClientFactory.CreateClient();
        var graphQlClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = endpoint.ApiUri }, _serializer, httpClient);

        graphQlClient.HttpClient.DefaultRequestHeaders.Add("sc_apikey", endpoint.ApiKey);
        graphQlClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);

        return graphQlClient;
    }
}
