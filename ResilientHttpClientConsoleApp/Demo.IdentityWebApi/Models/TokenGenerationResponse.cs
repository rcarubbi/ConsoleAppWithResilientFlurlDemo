namespace Demo.IdentityWebApi.Models
{
    public class TokenGenerationResponse
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}
