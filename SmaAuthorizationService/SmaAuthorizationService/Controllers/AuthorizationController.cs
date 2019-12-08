using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthCommonLib.Authorize;
using SmaAuthorizationService.Services.Interfaces;

namespace SmaAuthorizationService.Controllers
{

    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public AuthorizationController(IAuthorizationService authService)
        {
            authorizationService = authService;
        }


        [HttpPost("/checktoken")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<AuthorizeTokenResponse>), 200)]
        public ActionResult<AuthorizeTokenResponse> CheckToken([FromBody] AuthorizeTokenRequest request)
        {
            return authorizationService.CheckToken(request);
        }

    }
}