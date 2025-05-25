using Microsoft.AspNetCore.Mvc;
using Geno.Models;
using Geno.Services;

namespace Geno.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto.Email, dto.Password);
            if (!result.Success)
                return BadRequest(result.Errors);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto.Email, dto.Password);
            if (result.Requires2FA)
                return Ok(new { Requires2FA = true, result.UserId });
            if (!result.Success)
                return Unauthorized(result.Errors);
            return Ok(result);
        }

        [HttpPost("2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] TwoFactorDto dto)
        {
            var result = await _authService.Verify2FAAsync(dto.UserId, dto.Code);
            if (!result.Success)
                return Unauthorized(result.Errors);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.Token, dto.RefreshToken);
            if (!result.Success)
                return Unauthorized(result.Errors);
            return Ok(result);
        }
    }
}