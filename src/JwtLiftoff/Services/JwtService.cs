using JwtLiftoff.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace JwtLiftoff.Services
{
    public class JwtService
    {
        // Simple user storage consisting of username => password
        private static Dictionary<string, string> userStorage = new Dictionary<string, string>()
        {
            { "Default", "not-so-secret" },
            { "Brad Pitt", "se7en" },
            { "Rookie", "hello-world" }
        };

        public static Task<ClaimsIdentity> GetClaimsIdentity(UserIdentity user)
        {
            if(userStorage.ContainsKey(user.Username))
            {
                var userMatch = userStorage.FirstOrDefault(k => k.Key == user.Username);
                if (userMatch.Value == user.Password)
                {
                    return Task.FromResult(new ClaimsIdentity(
                        new GenericIdentity(user.Username, "Token"),
                        new List<Claim>()
                        {
                            new Claim("Claim", "Master")
                        }
                    ));
                }
            }

            return Task.FromResult<ClaimsIdentity>(null);
        }

        public static JwtSecurityToken SignJwtToken(JwtIssuerOptions options, IEnumerable<Claim> grantedClaims)
        {
            return new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                notBefore: options.NotBefore,
                expires: options.Expiration,
                signingCredentials: options.SigningCredentials,

                claims: grantedClaims
            );
        }

        public static string EncodeToString(JwtSecurityToken jwt)
        {
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static async Task<List<Claim>> GenerateClaimsForUserAsync(UserIdentity user, ClaimsIdentity identity, JwtIssuerOptions options)
        {
            return new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, await options.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeHelper.ToUnixEpochDate(options.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(user.Username)
            };
        }

        public static void ValidateJwtOptions(JwtIssuerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            else if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan", nameof(JwtIssuerOptions.ValidFor));
            else if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            else if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }
    }
}
