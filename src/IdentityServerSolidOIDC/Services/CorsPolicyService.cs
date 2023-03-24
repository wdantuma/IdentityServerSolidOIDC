using Duende.IdentityServer.Services;

namespace IdentityServerSolidOIDC.Services;

public class CorsPolicyService : ICorsPolicyService
{
    public async Task<bool> IsOriginAllowedAsync(string origin)
    {
        return true;
    }
}
