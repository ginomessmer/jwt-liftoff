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
        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult("https://youtu.be/0cmqwbZa6_w");
        }
    }
}
