using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyForgeApp.Models;

namespace MyForgeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public record AccessToken(string access_token, long expires_in);

        private readonly ForgeService _forgeService;

        public AuthController(ForgeService forgeService)
        {
            _forgeService = forgeService;
        }

        [HttpGet("token")]
        public async Task<AccessToken> GetAccessToken()
        {
            var token = await _forgeService.GetPublicToken();
            return new AccessToken(
                token.AccessToken,
                (long)Math.Round((token.ExpiresAt - DateTime.UtcNow).TotalSeconds)
            );
        }
    }
}
