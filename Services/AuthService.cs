using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Geno.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Geno.Services
{

    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;

        public AuthService(
            UserManager<User> userManager,
            IConfiguration config,
            IMailService mailService)
        {
            _userManager = userManager;
            _config = config;
            _mailService = mailService;
        }

        public async Task<AuthResult> RegisterAsync(string email, string password)
        {
            var user = new User { UserName = email, Email = email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return new AuthResult
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                };
            // await _userManager.AddToRoleAsync(user, "Customer");
            return new AuthResult { Success = true };
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return new AuthResult { Success = false, Errors = new[] { "Invalid credentials" } };

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
                var userEmail = user.Email ?? throw new InvalidOperationException("User has no email");
                await
                _mailService.SendAsync(userEmail, "Your 2FA Code", $"Your code: {code}");
                return new AuthResult { Success = false, Requires2FA = true, UserId = user.Id };
            }

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResult> Verify2FAAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new AuthResult { Success = false, Errors = new[] { "Invalid user" } };

            var valid = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, code);
            if (!valid)
                return new AuthResult { Success = false, Errors = new[] { "Invalid 2FA code" } };

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var email = principal?.Identity?.Name;
            var user = await _userManager.FindByEmailAsync(email!);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return new AuthResult { Success = false, Errors = new[] { "Invalid refresh token" } };

            return await GenerateTokensAsync(user);
        }

        private async Task<AuthResult> GenerateTokensAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new AuthResult { Success = true, Token = token, RefreshToken = newRefreshToken };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
                ValidateLifetime = false
            };
            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, parameters, out _);
            return principal;
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}