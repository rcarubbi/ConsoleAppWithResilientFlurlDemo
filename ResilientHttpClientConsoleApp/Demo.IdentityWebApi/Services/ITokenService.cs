namespace Demo.IdentityWebApi.Services;

public interface ITokenService
{
    (string AccessToken, string RefreshToken) GenerateTokens(string clientId, string clientSecret, Dictionary<string, object> customClaims);

    Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(string refreshToken);
}
