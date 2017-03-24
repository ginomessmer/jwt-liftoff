using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JwtLiftoff.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WhatsInTheBoxController : Controller
    {
        /// <summary>
        /// Find out what's in the box. Requires JWT auth, see example for further info
        /// </summary>
        /// <example><![CDATA[
        /// GET /api/whatsinthebox HTTP/1.1
        /// Host: localhost
        /// Authorization: Bearer <JWT>
        /// Cache-Control: no-cache
        /// ]]>
        /// </example>
        /// <returns>Secret stuff from the box</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult("https://youtu.be/0cmqwbZa6_w");
        }
    }
}
