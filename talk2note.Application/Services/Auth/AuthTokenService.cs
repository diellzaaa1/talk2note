using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.Auth
{
    public class AuthTokenService
    {
        private readonly string _jwtSecret;

        public AuthTokenService(string jwtSecret)
        {
            _jwtSecret = jwtSecret ?? throw new ArgumentNullException(nameof(jwtSecret), "JWT secret must be configured.");
        }

        public string GenerateToken(User user)
        {
            var issuedAt = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var expiration = new DateTimeOffset(DateTime.UtcNow.AddHours(24)).ToUnixTimeSeconds();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expiration.ToString()),
                new Claim("Role", "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
        
