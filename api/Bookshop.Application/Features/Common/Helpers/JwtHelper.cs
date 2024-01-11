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
        /// <summary>
        /// Generates a JWT (Json Web Token) for the given user using UserManager, user data, and JWT settings.
        /// </summary>
        /// <param name="userManager">User manager for managing user-related operations.</param>
        /// <param name="user">User data for whom the token is generated.</param>
        /// <param name="jwtSettings">JWT settings containing issuer, audience, key, and duration.</param>
        /// <returns>Returns a JwtSecurityToken if successful; otherwise, returns null.</returns>
        public static async Task<JwtSecurityToken?> GenerateToken(
            UserManager<IdentityUserData> userManager,
            IdentityUserData user,
            JwtSettings jwtSettings)
        {
            // Retrieve user roles and claims
            var userRoles = await userManager.GetRolesAsync(user);
            var userClaims = await userManager.GetClaimsAsync(user);

            // Build the claims for the JWT
            var claims = BuildClaims(user, userRoles, userClaims);

            // Check if a key is provided in the JWT settings
            if (!string.IsNullOrEmpty(jwtSettings.Key))
            {
                // Convert the base64-encoded key to raw bytes and create a symmetric security key
                var rawKeyBytesOfBase64EncodedKey = Convert.FromBase64String(jwtSettings.Key);
                var symmetricSecurityKey = new SymmetricSecurityKey(rawKeyBytesOfBase64EncodedKey);

                // Create signing credentials using HMACSHA256 algorithm
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                // Create and return a JWT security token
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
                    signingCredentials: signingCredentials);

                return jwtSecurityToken;
            }

            // Return null if no key is provided
            return null;
        }

        /// <summary>
        /// Builds the claims for the JWT, including standard claims, user-specific claims, and role claims.
        /// </summary>
        /// <param name="user">User data for whom the claims are built.</param>
        /// <param name="userRoles">List of roles associated with the user.</param>
        /// <param name="userClaims">List of additional claims associated with the user.</param>
        /// <returns>Returns a collection of claims for the JWT.</returns>
        private static IEnumerable<Claim> BuildClaims(
            IdentityUserData user,
            IList<string> userRoles,
            IList<Claim> userClaims)
        {
            // Standard claims for the JWT
            var standardClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
        };

            // Role claims for the JWT
            var roleClaims = userRoles.Select(role => new Claim("role", role));

            // Combine standard claims, user-specific claims, and role claims
            return standardClaims
                .Union(userClaims)
                .Union(roleClaims);
        }
    }

}
