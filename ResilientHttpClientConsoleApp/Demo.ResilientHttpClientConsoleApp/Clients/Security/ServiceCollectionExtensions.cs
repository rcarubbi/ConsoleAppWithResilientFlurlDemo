using Carubbi.ResilientFlurl.Extensions;
using Demo.ResilientHttpClientConsoleApp.Clients.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.ResilientHttpClientConsoleApp.Clients.Security;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtTokenAuth<T>(this IServiceCollection services, IConfiguration configuration)
    {
        var name = typeof(T).Name;
        services.Configure<HttpClientSecurityOptions>(name, configuration.GetSection($"{name}:HttpClient"));
        services.AddSingleton<ITokenProvider<T>, TokenProvider<T>>();
        services.AddSingleton<JwtDelegatingHandler<T>>();
        services.AddResilientHttpClient<TokenProvider<T>>(configuration);
        return services;
    }
}
