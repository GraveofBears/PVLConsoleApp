using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AuthServerTool.Utils
{
    public static class AuthUtils
    {
        // 🔧 Generate a lean JWT token with timing claims
        public static string GenerateJwt(string username, string secretKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: "AuthServerTool",
                audience: "AppClient",
                claims: new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Iat,
                              new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                },
                notBefore: now,
                expires: now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // 🔍 Validate incoming token and return claims if valid
        public static ClaimsPrincipal? ValidateJwt(string token, string secretKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "AuthServerTool",

                    ValidateAudience = true,
                    ValidAudience = "AppClient",

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // 🕒 Strict expiration check
                }, out _);

                return principal;
            }
            catch
            {
                return null; // ❌ Invalid token
            }
        }
    }
}
