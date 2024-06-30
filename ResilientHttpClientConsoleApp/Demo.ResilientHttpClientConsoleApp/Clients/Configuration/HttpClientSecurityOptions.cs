namespace Demo.ResilientHttpClientConsoleApp.Clients.Configuration;

public class HttpClientSecurityOptions
{
    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public string? AccessTokenEndpoint { get; set; }
    public string? RefreshTokenEndpoint { get; set; }

    public string? TokenProviderAddress { get; set; }
}
