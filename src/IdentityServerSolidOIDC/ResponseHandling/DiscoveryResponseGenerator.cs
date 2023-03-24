// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityModel;
using Duende.IdentityServer.Stores;
using Microsoft.Extensions.Logging;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Duende.IdentityServer.ResponseHandling;

namespace IdentityServerSolidOIDC;

/// <summary>
/// Solid OIDC implementation of the discovery endpoint response generator
/// </summary>
/// <seealso cref="IDiscoveryResponseGenerator" />
public class DiscoveryResponseGenerator : Duende.IdentityServer.ResponseHandling.DiscoveryResponseGenerator
{
    IdentityServerOptions _options;

    public DiscoveryResponseGenerator(IdentityServerOptions options,
        IResourceStore resourceStore,
        IKeyMaterialService keys,
        ExtensionGrantValidator extensionGrants,
        ISecretsListParser secretParsers,
        IResourceOwnerPasswordValidator resourceOwnerValidator,
        ILogger<DiscoveryResponseGenerator> logger) : base(options, resourceStore, keys, extensionGrants, secretParsers, resourceOwnerValidator, logger) 
    {
        _options = options;
    }

    public async override Task<Dictionary<string, object>> CreateDiscoveryDocumentAsync(string baseUrl, string issuerUri)
    {
        var response = await base.CreateDiscoveryDocumentAsync(baseUrl, issuerUri);
        // add webid to supported claims and scopes
        if(_options.Solid.Enabled)
        {
            if (response.ContainsKey(OidcConstants.Discovery.ScopesSupported))
            {
                var supportedScopes = new List<string>(response[OidcConstants.Discovery.ScopesSupported] as string[]);
                supportedScopes.Add(SolidOIDCConstants.WebIdScope);
                response[OidcConstants.Discovery.ScopesSupported] = supportedScopes.ToArray();
            }
            if(response.ContainsKey(OidcConstants.Discovery.ClaimsSupported))
            {
                var supportedClaims = new List<string>(response[OidcConstants.Discovery.ClaimsSupported] as string[]);
                supportedClaims.Add(SolidOIDCConstants.WebIdScope);
                response[OidcConstants.Discovery.ClaimsSupported] = supportedClaims.ToArray();
            }
          
        }
        return response;
    }
}