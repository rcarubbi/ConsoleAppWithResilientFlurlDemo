namespace Demo.IdentityWebApi.Models;

public class TokenGenerationRequest
{
    public string? Email { get; set; }
    public Guid UserId { get; set; }

    public Dictionary<string, object> CustomClaims { get; set; } = [];
}
