using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthCommonLib.Authenticate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmaAuthorizationService.Repositories.Interfaces;
using SmaAuthorizationService.Services.Interfaces;

namespace SmaAuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService authenticateService;
        private readonly ILogger<AuthenticateController> logger;

        public AuthenticateController(IAuthenticateService authenticateService, ILogger<AuthenticateController> logger)
        {
            this.authenticateService = authenticateService;
            this.logger = logger;
        }

        [HttpPost("/login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<AuthenticateResponse>), 200)]
        public ActionResult<AuthenticateResponse> Login([FromBody] AuthenticateRequest loginInfo)
        {
            try
            {
                var loginResult = authenticateService.Login(loginInfo);

                if (loginResult == null)
                {
                    Response.StatusCode = 401;
                    return new AuthenticateResponse();
                }

                return loginResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка");
                Response.StatusCode = 500;
                return new AuthenticateResponse();
            }



        }
    }
}