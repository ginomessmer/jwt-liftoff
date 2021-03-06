﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using JwtLiftoff.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using JwtLiftoff.Common.Swagger;

namespace JwtLiftoff.Services
{
    public class ConfigurationService
    {
        // Keep this secret, will ya
        internal static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("TotallySecretSigningKey"));

        internal const string ISSUER = "UberJwtServer";
        internal const string AUDIENCE = "https://localhost/";

        #region Methods
        public static void AddMvcConfig(IServiceCollection services)
        {
            services.AddMvc(config => 
            {
                // Require auth per default
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
                options.Issuer = ISSUER;
                options.Audience = AUDIENCE;
                options.SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
            });
        }

        public static void AddSwagger(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Title = "JWT Liftoff",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact() { Url = "https://github.com/ginomessmer/jwt-liftoff" },
                    Description = "JWT sample based on ASP.NET Core"
                });

                config.OperationFilter<JwtAuthorizationFilter>();
            });
        }

        public static void EnableSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT Liftoff");
            });
        }

        public static void AddAuthorizationPolicies(IServiceCollection services, List<string> policyNames)
        {
            foreach(string name in policyNames)
            {
                services.AddAuthorization(options => 
                {
                    // Add policies like 'UberPolicy' by 'Policy' => 'Uber'
                    options.AddPolicy($"{name}Policy", policy => policy.RequireClaim("Claim", name));
                });
            }
        }

        public static void AddJwtAppBuilderConfiguration(IApplicationBuilder app)
        {
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,

                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = ISSUER,               // Again, hardcoded. Must match with Issuer specified in ConfigureJwt(...)

                    ValidateAudience = true,
                    ValidAudience = AUDIENCE,           // Again, same

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SigningKey,      // Secret signing key for validation

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                }
            });
        }

        #endregion
    }
}
