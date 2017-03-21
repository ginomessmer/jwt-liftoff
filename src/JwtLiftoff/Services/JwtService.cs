using JwtLiftoff.Data;
using System;
using System.Collections.Generic;
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
            { "Client", "biz" },
            { "Rookie", "hello-world" }
        };

        public static Task<ClaimsIdentity> GetClaimsIdentity(UserIdentity user)
        {
            if(userStorage.ContainsKey(user.Username))
            {
                var userMatch = userStorage.FirstOrDefault(k => k.Key == user.Username);
                if (userMatch.Value == user.Username)
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
