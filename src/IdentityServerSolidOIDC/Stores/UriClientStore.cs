using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using IdentityServerSolidOIDC.Extensions;
using IdentityServerSolidOIDC.Services;

namespace IdentityServerSolidOIDC.Stores;

public class UriClientStore : IClientStore
{
    ClientIdUriHttpClient _clientIdUriHttpClient;
    public UriClientStore(ClientIdUriHttpClient clientIdUriHttpClient)
    {
        _clientIdUriHttpClient = clientIdUriHttpClient;
    }
    public async Task<Client> FindClientByIdAsync(string clientId)
    {
        if(clientId.IsUri())
        {
            return await _clientIdUriHttpClient.GetClientIDDocumentAsync(clientId);
        }

        return null;
    }
}
