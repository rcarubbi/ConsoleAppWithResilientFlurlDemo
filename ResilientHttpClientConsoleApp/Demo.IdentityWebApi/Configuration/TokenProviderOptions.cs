namespace Demo.IdentityWebApi.Configuration;

public class TokenProviderOptions
{
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
   
    public TimeSpan AccessTokenLifeTime { get; set; }
    public TimeSpan RefreshTokenLifeTime { get; set; }
}
