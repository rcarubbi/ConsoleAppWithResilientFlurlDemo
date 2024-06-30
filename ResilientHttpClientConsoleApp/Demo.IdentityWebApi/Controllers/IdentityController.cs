using Demo.IdentityWebApi.Models;
using Demo.IdentityWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Demo.IdentityWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(ITokenService tokenService) : ControllerBase
    {
        [HttpPost]
        [Route("token")]
        public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
        {
            var (accessToken, refreshToken) = tokenService.GenerateTokens(request.Email!, request.UserId.ToString(), request.CustomClaims);
            return Ok(new TokenGenerationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var (accessToken, refreshToken) = await tokenService.RefreshTokensAsync(request.RefreshToken!);
                return Ok(new TokenGenerationResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch(SecurityTokenException ex)
            {
                return BadRequest(ex.Message);
            }
          
        }
    }
}
