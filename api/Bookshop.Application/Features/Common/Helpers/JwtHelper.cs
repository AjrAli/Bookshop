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
        public static JwtSecurityToken? GenerateToken(UserManager<IdentityUserData> userManager, IdentityUserData user, JwtSettings jwtSettings)
        {
            var userRoles = userManager.GetRolesAsync(user).Result;
            var userClaims = userManager.GetClaimsAsync(user).Result;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            }
            .Union(userClaims)
            .Union(userRoles.Select(role => new Claim("role", role))); // Add the roles as claims

            if (jwtSettings.Key != null)
            {
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                // Adjust token expiration as needed (e.g., 1 hour)
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10), // Change to your desired expiration time
                    signingCredentials: signingCredentials);

                return jwtSecurityToken;
            }

            return null;
        }
    }
}
