﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JwtLiftoff.Data;
using Microsoft.Extensions.Options;
using JwtLiftoff.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtLiftoff.Controllers
{
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        private readonly JwtIssuerOptions jwtOptions;
        private readonly ILogger logger;

        public JwtController(IOptions<JwtIssuerOptions> options, ILoggerFactory loggerFactory)
        {
            this.jwtOptions = options.Value;
            JwtService.ValidateJwtOptions(this.jwtOptions);
            this.logger = loggerFactory.CreateLogger<JwtController>();
        }

        #region Methods

        /// <summary>
        /// Returns a new JWT token based on the username and password sent through headers.
        /// Make sure to set Body to x-www-form-urlencoded
        /// </summary>
        /// <example><![CDATA[
        /// POST /api/jwt HTTP/1.1
        /// Host: localhost
        /// Content-Type: application/x-www-form-urlencoded
        /// Cache-Control: no-cache
        /// username = Rookie & password = hello - world
        /// ]]>
        /// </example>
        /// <param name="user"></param>
        /// <returns>JWT token for further use in controllers with require auth</returns>
        [HttpPost]          // We'll receive their sign in credentials to validate
        [AllowAnonymous]    // Guests need to receive their JWT tokens
        public async Task<IActionResult> Get([FromForm] UserIdentity user)
        {
            var identity = await JwtService.GetClaimsIdentity(user);

            if(identity == null)
                return BadRequest($"Invalid user {user.Username}. Check credentials again");

            List<Claim> claims = await JwtService.GenerateClaimsForUserAsync(user, identity, this.jwtOptions);
            JwtSecurityToken jwt = JwtService.SignJwtToken(this.jwtOptions, claims);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(jwt); 

            return new OkObjectResult(encodedToken);
        }

        #endregion


        #region Helper methods

        #endregion
    }
}
