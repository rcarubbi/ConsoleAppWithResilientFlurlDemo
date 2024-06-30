using Microsoft.AspNetCore.Authorization;

namespace Demo.WebApi.Security;

public class TokenTypeHandler : AuthorizationHandler<TokenTypeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenTypeRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == "token_type"))
        {
            var tokenType = context.User.FindFirst(c => c.Type == "token_type")!.Value;
            if (tokenType == requirement.RequiredTokenType)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
