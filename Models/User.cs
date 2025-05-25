using Microsoft.AspNetCore.Identity;
using System;

namespace Geno.Models
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}