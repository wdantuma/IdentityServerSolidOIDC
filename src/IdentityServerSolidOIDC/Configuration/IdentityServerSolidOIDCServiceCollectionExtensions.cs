using Duende.IdentityServer.ResponseHandling;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityServerSolidOIDC.Services;
using IdentityServerSolidOIDC.SolidOIDC.Services;
using IdentityServerSolidOIDC.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace IdentityServerSolidOIDC;

public static class IdentityServerSolidOIDCServiceCollectionExtensions
{

    public static IIdentityServerBuilder AddIdentityServerSolidOIDC(this IServiceCollection services)
    {
        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<IdentityServerOptions>>().Value);
        services.AddSingleton<IOptions<Duende.IdentityServer.Configuration.IdentityServerOptions>>(resolver => resolver.GetRequiredService<IOptions<IdentityServerOptions>>());
        services.TryAddTransient<ICorsPolicyService, CorsPolicyService>();
        services.TryAddTransient<IDiscoveryResponseGenerator, DiscoveryResponseGenerator>();
        services.TryAddTransient<ITokenService, TokenService>();
        services.TryAddTransient<ClientIdUriHttpClient, ClientIdUriHttpClient>();
        services.TryAddTransient<IClientStore, UriClientStore>();
        return services.AddIdentityServer();
    }
    public static IIdentityServerBuilder AddIdentityServerSolidOIDC(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
    {
        services.Configure(setupAction);
        return services.AddIdentityServerSolidOIDC();
    }

    public static IIdentityServerBuilder AddIdentityServerSolidOIDC(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerOptions>(configuration);
        return services.AddIdentityServerSolidOIDC();
    }
}
