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
            var client = new Client();
            client.ClientId = clientIdDocument.GetString("client_id");
            client.ClientName = clientIdDocument.GetString("client_name");
            client.RedirectUris = new Collection<string>(clientIdDocument.TryGetStringArray("redirect_uris").ToList());
            client.AllowedGrantTypes = new Collection<string>(clientIdDocument.TryGetStringArray("grant_types").ToList());
            var scopes = new List<string>((clientIdDocument.GetString("scope") ?? "").Split(" "));
            scopes.Add("profile");
            client.AllowedScopes =  new Collection<string>(scopes);
            client.AllowOfflineAccess = true;
            client.RequireClientSecret = false;
            client.AllowRememberConsent = true;
            client.RequireConsent = true;
            client.AllowPlainTextPkce = false;
            client.RequirePkce = true;
            client.RequireDPoP = true;
            return client;
        }
        return null;
    }
}
