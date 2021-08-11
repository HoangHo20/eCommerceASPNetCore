using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet()]
        public IActionResult Login()
        {

            //var authProperties = _signInManager
            //.ConfigureExternalAuthenticationProperties("oidc",
            //Url.Action("LoggingIn", "Account", null, Request.Scheme));

            //return Challenge(authProperties, "oidc");
            return Ok("helloworld");
        }
    }
}
