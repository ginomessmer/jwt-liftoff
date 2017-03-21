using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using JwtLiftoff.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtLiftoff.Services
{
    public class ConfigurationService
    {
        public static void AddMvcConfig(IServiceCollection services)
        {
            services.AddMvc(config => 
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        public static void ConfigureJwt(IServiceCollection services)
        {
            services.Configure<JwtIssuerOptions>(options =>
            {
                // String options are hardcoded in this sample
                options.Issuer = "UberJwtServer";
                options.Audience = "https://localhost/";
                options.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("TotallySecret")), 
                    SecurityAlgorithms.HmacSha256);
            });
        }

        public static void AddAuthorizationPolicies(IServiceCollection services, List<string> policyNames)
        {
            foreach(string name in policyNames)
            {
                services.AddAuthorization(options => 
                {
                    options.AddPolicy($"{name}Policy", policy => policy.RequireClaim("Policy", name));
                });
            }
        }
    }
}
