using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtLiftoff.Data
{
    /// <summary>
    /// Simple user data model consisting of username and password
    /// </summary>
    public class UserIdentity
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
