using Demo.ResilientHttpClientConsoleApp.Clients.Configuration;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Demo.ResilientHttpClientConsoleApp.Clients.Security;

public class TokenProvider<T>(IOptionsSnapshot<HttpClientSecurityOptions> settings, IServiceProvider serviceProvider) : ITokenProvider<T>
{
    private readonly HttpClientSecurityOptions _settings = settings.Get(typeof(T).Name);
    private string? _accessToken;
    private string? _refreshToken;
    private IFlurlClient? _flurlClient;
    private SemaphoreSlim _semaphoreSlim = new(1, 1);
    public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _semaphoreSlim.WaitAsync(cancellationToken);
            if (string.IsNullOrEmpty(_accessToken) || TokenExpired(_accessToken))
            {
                await RefreshOrFetchTokenAsync(cancellationToken);
            }
            return _accessToken!;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private static bool TokenExpired(string token)
    {
        var jwtToken = new JsonWebTokenHandler().ReadToken(token);
        return jwtToken?.ValidTo < DateTime.UtcNow;
    }

    private async Task RefreshOrFetchTokenAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_refreshToken))
        {
            try
            {
                await RefreshTokenAsync(cancellationToken);
                return;
            }
            catch (Exception)
            {
                // Ignored, will fallback to fetching a new token
            }
        }
        await FetchNewTokenAsync(cancellationToken);
    }

    private async Task FetchNewTokenAsync(CancellationToken cancellationToken)
    {
        _flurlClient ??= serviceProvider.GetRequiredKeyedService<IFlurlClient>($"TokenProvider<{typeof(T).Name}>");
        var response = await _flurlClient.Request(_settings.AccessTokenEndpoint)
            .AllowHttpStatus(400)
            .PostJsonAsync(new { email = _settings.ClientId, userId = _settings.ClientSecret }, cancellationToken: cancellationToken)
            .ReceiveJson<TokenResponse>();

        _accessToken = response.AccessToken;
        _refreshToken = response.RefreshToken;
    }

    private async Task RefreshTokenAsync(CancellationToken cancellationToken)
    {
        _flurlClient ??= serviceProvider.GetRequiredKeyedService<IFlurlClient>($"TokenProvider<{typeof(T).Name}>");
        var response = await _flurlClient.Request(_settings.RefreshTokenEndpoint)
            .AllowHttpStatus(400)
            .PostJsonAsync(new { refreshToken = _refreshToken }, cancellationToken: cancellationToken)
            .ReceiveJson<TokenResponse>();

        _accessToken = response.AccessToken;
        _refreshToken = response.RefreshToken;
    }
}
