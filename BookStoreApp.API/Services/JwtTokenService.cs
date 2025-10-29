using BookStoreApp.API.Common.Constants;
using BookStoreApp.API.Data;
using BookStoreApp.API.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApp.API.Services
{
    public sealed class JwtTokenService
    {
        private readonly JwtSettingsOptions _jwt;
        private readonly UserManager<ApiUser> _userManager;

        public JwtTokenService(IOptions<JwtSettingsOptions> jwtOptions, UserManager<ApiUser> userManager)
        {
            _jwt = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenAsync(ApiUser user, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_jwt.Key))
                throw new InvalidOperationException("JwtSettings:Key is missing.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName ?? user.Email ?? user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(CustomClaimTypes.UUID, user.Id)
        };
            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(_jwt.Duration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
