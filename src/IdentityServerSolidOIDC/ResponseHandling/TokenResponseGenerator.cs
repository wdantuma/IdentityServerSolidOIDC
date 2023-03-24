using Duende.IdentityServer.ResponseHandling;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServerSolidOIDC;

internal class TokenResponseGenerator : ITokenResponseGenerator
{
    private Duende.IdentityServer.ResponseHandling.TokenResponseGenerator wrappedTokenGenerator;

    public TokenResponseGenerator(ISystemClock clock, ITokenService tokenService, IRefreshTokenService refreshTokenService, IScopeParser scopeParser, IResourceStore resources, IClientStore clients, ILogger<Duende.IdentityServer.ResponseHandling.TokenResponseGenerator> logger)
    {
       wrappedTokenGenerator = new Duende.IdentityServer.ResponseHandling.TokenResponseGenerator(clock,tokenService,refreshTokenService,scopeParser,resources,clients,logger);
    }
    public async Task<TokenResponse> ProcessAsync(TokenRequestValidationResult validationResult)
    {
        return await wrappedTokenGenerator.ProcessAsync(validationResult);
    }
}
