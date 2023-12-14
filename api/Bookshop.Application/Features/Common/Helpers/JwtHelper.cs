using Bookshop.Application.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Application.Features.Common.Helpers
{
    public static class JwtHelper
    {
        public static async Task<JwtSecurityToken?> GenerateToken(
            UserManager<IdentityUserData> userManager,
            IdentityUserData user,
            JwtSettings jwtSettings)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var userClaims = await userManager.GetClaimsAsync(user);

            var claims = BuildClaims(user, userRoles, userClaims);

            if (!string.IsNullOrEmpty(jwtSettings.Key))
            {
                var rawKeyBytesOfBase64EncodedKey = Convert.FromBase64String(jwtSettings.Key);
                var symmetricSecurityKey = new SymmetricSecurityKey(rawKeyBytesOfBase64EncodedKey);
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
                    signingCredentials: signingCredentials);

                return jwtSecurityToken;
            }

            return null;
        }

        private static IEnumerable<Claim> BuildClaims(
            IdentityUserData user,
            IList<string> userRoles,
            IList<Claim> userClaims)
        {
            var standardClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var roleClaims = userRoles.Select(role => new Claim("role", role));

            return standardClaims
                .Union(userClaims)
                .Union(roleClaims);
        }
    }
}
