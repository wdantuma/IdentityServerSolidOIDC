namespace IdentityServerSolidOIDC;

public class IdentityServerOptions : Duende.IdentityServer.Configuration.IdentityServerOptions
{      
    /// <summary>
    /// Options for Solid
    /// </summary>
    public SolidOptions Solid { get; set; } = new SolidOptions();
}