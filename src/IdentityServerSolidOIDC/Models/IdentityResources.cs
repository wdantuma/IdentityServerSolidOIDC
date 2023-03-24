using Duende.IdentityServer.Models;

namespace IdentityServerSolidOIDC;

public static class IdentityResources
{
    public class WebId : IdentityResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebId"/> class.
        /// </summary>
        public WebId()
        {
            Name = SolidOIDCConstants.WebIdScope;
            DisplayName = "Your webid";
            Required = true;
            UserClaims.Add(JwtClaimTypes.WebId);
        }
    }
}
