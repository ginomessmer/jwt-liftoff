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
        /// <summary>
        /// Username, also principal of this user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
