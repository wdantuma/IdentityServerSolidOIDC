using Duende.IdentityServer.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace IdentityServerSolidOIDC.Services;

public class ClientIdUriHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IdentityServerOptions _options;
    private readonly ILogger<ClientIdUriHttpClient> _logger;

    public ClientIdUriHttpClient(HttpClient httpClient, IdentityServerOptions options, ILoggerFactory loggerFactory)
    {
        _httpClient = httpClient;
        _options = options;
        _logger = loggerFactory.CreateLogger<ClientIdUriHttpClient>();
    }

    public async Task<Client> GetClientIDDocumentAsync(string url)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _httpClient.SendAsync(req);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();
            var clientIdDocument = JsonSerializer.Deserialize<JsonElement>(contentStream);
            var scopes = new List<string>((clientIdDocument.GetString("scope") ?? "").Split(" "));
            scopes.Add("profile");
            var client = new Client()
            {
                ClientId = clientIdDocument.GetString("client_id"),
                ClientName = clientIdDocument.GetString("client_name"),
                RedirectUris = new Collection<string>(clientIdDocument.TryGetStringArray("redirect_uris").ToList()),
                AllowedGrantTypes = new Collection<string>(clientIdDocument.TryGetStringArray("grant_types").ToList()),
                AllowedIdentityTokenSigningAlgorithms = { "ES256" },
                AllowedScopes = new Collection<string>(scopes),
                AllowOfflineAccess = true,
                RequireClientSecret = false,
                AllowRememberConsent = true,
                RequireConsent = true,
                AllowPlainTextPkce = false,
                RequirePkce = true,
                RequireDPoP = true
            };

            return client;
        }
        return null;
    }
}
