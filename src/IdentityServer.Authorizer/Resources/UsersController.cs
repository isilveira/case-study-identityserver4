using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Authorizer.Resources
{
    [Route("api/v1/[controller]")]
    public class UsersController: ControllerBase
    {
        [HttpGet]
        public ActionResult<List<string>> Get()
        {
            return new List<string> { "user1", "user2", "user3" };
        }
    }
}
