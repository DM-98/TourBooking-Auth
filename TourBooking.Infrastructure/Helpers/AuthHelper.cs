using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TourBooking.Infrastructure.Helpers;

public static class AuthHelper
{
    public static string GenerateAccessToken(IEnumerable<Claim> claims, string secret, string issuer, DateTime accessTokenExpiry)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        var jwtSecurityToken = new JwtSecurityToken(issuer: issuer, claims: claims, expires: accessTokenExpiry, signingCredentials: signingCredentials);
        var accesstoken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return accesstoken;
    }

    public static string GenerateRefreshToken()
    {
        var random = new byte[64];

        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(random);

        var refreshToken = Convert.ToBase64String(random);

        return refreshToken;
    }

    public static async Task<ClaimsPrincipal?> TryGetClaimsPrincipalAsync(string accessToken, string secret, string issuer, string audience)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            IssuerSigningKey = symmetricSecurityKey,
            ValidIssuer = issuer,
            ValidAudience = audience,
            ClockSkew = TimeSpan.Zero
        };

        var tokenValidationResult = await new JwtSecurityTokenHandler().ValidateTokenAsync(accessToken, tokenValidationParameters);

        return tokenValidationResult.IsValid && tokenValidationResult.SecurityToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase)
            ? new ClaimsPrincipal(tokenValidationResult.ClaimsIdentity)
            : null;
    }

    public static IEnumerable<Claim> ParseClaimsFromJWT(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var bytes = Convert.FromBase64String(payload.Length % 4 is 2 ? payload += "==" : payload.Length % 4 is 3 ? payload += "=" : payload);
        var keyValue = JsonSerializer.Deserialize<Dictionary<string, string>>(bytes);

        if (keyValue is not null)
        {
            var roles = keyValue.Where(x => x.Key is "role").Select(x => x.Value).ToList();

            if (roles?.Any() ?? false)
            {
                roles.ForEach(role => role.Trim().TrimStart('[').TrimEnd(']').Split(','));

                if (roles.Count > 1)
                {
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim("role", role.Trim('"')));
                    }
                }
                else
                {
                    claims.Add(new Claim("role", roles.First()));
                }

                keyValue.Remove("role");
            }

            claims.AddRange(keyValue.Select(x => new Claim(x.Key, x.Value)));
        }

        return claims;
    }
}