using Demo.IdentityWebApi.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Demo.IdentityWebApi.Services;

public class TokenService : ITokenService
{
    private readonly byte[] _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _accessTokenLifeTime;
    private readonly TimeSpan _refreshTokenLifeTime;
    private const string TOKEN_TYPE_CLAIM = "token_type";
    public TokenService(IOptions<TokenProviderOptions> tokenProviderOptions)
    {
        var tokenProviderSettings = tokenProviderOptions.Value;
        _key = Encoding.UTF8.GetBytes(tokenProviderSettings.Key!);
        _issuer = tokenProviderSettings.Issuer!;
        _audience = tokenProviderSettings.Audience!;
        _accessTokenLifeTime = tokenProviderSettings.AccessTokenLifeTime!;
        _refreshTokenLifeTime = tokenProviderSettings.RefreshTokenLifeTime!; 
    }

    public (string AccessToken, string RefreshToken) GenerateTokens(string clientId, string clientSecret, Dictionary<string, object> customClaims)
    {
        var claims = GenerateClaims(clientId, customClaims);
        return (GenerateAccessToken(claims), GenerateRefreshToken(claims));
    }

    public async Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(string refreshToken)
    {
        var claimsIdentity = await ValidateTokenAsync(refreshToken);

        if (!claimsIdentity.HasClaim(c => c.Type == "token_type" && c.Value == "refresh")) throw new SecurityTokenException("Invalid refresh token");

        var userId = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        return userId == null
            ? throw new SecurityTokenException("Invalid refresh token")
            : ((string AccessToken, string RefreshToken))(GenerateAccessToken(claimsIdentity.Claims), GenerateRefreshToken(claimsIdentity.Claims));
    }

    private string GenerateRefreshToken(IEnumerable<Claim> claims)
    {
        var refreshClaims = claims.ToList();

        var tokenTypeClaim = refreshClaims.Find(x => x.Type == TOKEN_TYPE_CLAIM);
        refreshClaims.Remove(tokenTypeClaim!);
        refreshClaims.Add(new Claim(TOKEN_TYPE_CLAIM, "refresh"));

        var tokenHandler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(refreshClaims),
            Expires = DateTime.UtcNow.Add(_refreshTokenLifeTime),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }

    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var claimsList = claims.ToList();
        var tokenTypeClaim = claimsList.Find(x => x.Type == TOKEN_TYPE_CLAIM);
        claimsList.Remove(tokenTypeClaim!);
        claimsList.Add(new Claim(TOKEN_TYPE_CLAIM, "access"));

        var tokenHandler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claimsList),
            Expires = DateTime.UtcNow.Add(_accessTokenLifeTime),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }

    private static List<Claim> GenerateClaims(string clientId, Dictionary<string, object> customClaims)
    {
        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti.ToString(), Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, clientId!),
                new(JwtRegisteredClaimNames.Email, clientId!),
                new(TOKEN_TYPE_CLAIM, "")
            };

        foreach (var claimPair in customClaims)
        {
            var jsonElement = (JsonElement)claimPair.Value;
            var valueType = jsonElement.ValueKind switch
            {
                JsonValueKind.True => ClaimValueTypes.Boolean,
                JsonValueKind.False => ClaimValueTypes.Boolean,
                JsonValueKind.Number => ClaimValueTypes.Double,
                _ => ClaimValueTypes.String
            };

            var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);
            claims.Add(claim);
        }

        return claims;
    }


    public async Task<ClaimsIdentity> ValidateTokenAsync(string token)
    {
        var tokenHandler = new JsonWebTokenHandler();
       
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(_key),
            ClockSkew = TimeSpan.Zero
        };

        var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, validationParameters);
         
        if (!tokenValidationResult.IsValid)
        {
            throw tokenValidationResult.Exception;
        }

        return tokenValidationResult.ClaimsIdentity;
    }
}
