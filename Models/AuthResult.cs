namespace Geno.Models
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public bool Requires2FA { get; set; } = false;
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}