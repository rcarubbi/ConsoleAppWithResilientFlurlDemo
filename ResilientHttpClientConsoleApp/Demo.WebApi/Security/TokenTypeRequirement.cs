using Microsoft.AspNetCore.Authorization;

namespace Demo.WebApi.Security;

public class TokenTypeRequirement : IAuthorizationRequirement
{
    public string RequiredTokenType { get; }

    public TokenTypeRequirement(string requiredTokenType)
    {
        RequiredTokenType = requiredTokenType;
    }
}
