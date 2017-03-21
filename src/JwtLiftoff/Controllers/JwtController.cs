using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JwtLiftoff.Data;
using Microsoft.Extensions.Options;
using JwtLiftoff.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtLiftoff.Controllers
{
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        private readonly JwtIssuerOptions jwtOptions;

        public JwtController(IOptions<JwtIssuerOptions> options)
        {
            this.jwtOptions = options.Value;
            JwtService.ValidateJwtOptions(this.jwtOptions);
        }

        #region Methods

        // GET: api/values
        [HttpPost]          // We'll receive their sign in credentials to validate
        [AllowAnonymous]    // Guests need to receive their JWT tokens
        public string Get([FromForm] UserIdentity user)
        {
            var identity = await JwtService.GetClaimsIdentity(user);
        }

        #endregion


        #region Helper methods

        #endregion
    }
}
