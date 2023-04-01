using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonWebKey = Microsoft.IdentityModel.Tokens.JsonWebKey;

namespace IdentityServerSolidOIDC.SolidOIDC.Services
{
    public class TokenService : DefaultTokenService
    {
        private IHttpContextAccessor _contextAccessor;
        private IdentityServerOptions _options;
        private IClaimsService _claimsService;
        public TokenService(IClaimsService claimsProvider,
        IReferenceTokenStore referenceTokenStore,
        ITokenCreationService creationService,
        IHttpContextAccessor contextAccessor,
        ISystemClock clock,
        IKeyMaterialService keyMaterialService,
        IdentityServerOptions options,
        IClaimsService claimsService,
        ILogger<DefaultTokenService> logger) : base(claimsProvider, referenceTokenStore, creationService, contextAccessor, clock, keyMaterialService, options, logger)
        {
            _contextAccessor = contextAccessor;
            _options = options;
            _claimsService = claimsService;
        }

        private async Task<Token> AddSolidClaims(Token token, TokenCreationRequest request)
        {
            if (_options.Solid.Enabled)
            {
                // add azp and webid claims
                token.Claims.Add(new Claim(IdentityModel.JwtClaimTypes.AuthorizedParty, request.ValidatedRequest.Client.ClientId));
                var profileRequest = new ProfileDataRequestContext()
                {
                    Client = request.ValidatedRequest.Client,
                    Subject = request.ValidatedRequest.Subject
                };
                var webIdclaims = (await ClaimsProvider.GetIdentityTokenClaimsAsync(
                  request.Subject,
                  request.ValidatedResources,
                  true,
                  request.ValidatedRequest)).Where(c => c.Type == JwtClaimTypes.WebId);
                if (webIdclaims.Any() && !token.Claims.Where(c => c.Type == JwtClaimTypes.WebId).Any())
                {
                    token.Claims.Add(webIdclaims.First());
                }
                token.Audiences.Add("solid");

                // add cnf if present
                if (!string.IsNullOrEmpty(request.ValidatedRequest.Confirmation))
                {
                    token.Confirmation = request.ValidatedRequest.Confirmation;
                }
            }

            return token;
        }

        public override async Task<Token> CreateIdentityTokenAsync(TokenCreationRequest request)
        {
            var token = await base.CreateIdentityTokenAsync(request);
            return await AddSolidClaims(token, request);
        }

        public override async Task<Token> CreateAccessTokenAsync(TokenCreationRequest request)
        {
            var token = await base.CreateAccessTokenAsync(request);
            return await AddSolidClaims(token, request);           
        }
    }
}
