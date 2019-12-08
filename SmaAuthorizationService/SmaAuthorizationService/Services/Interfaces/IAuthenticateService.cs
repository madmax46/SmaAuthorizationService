using AuthCommonLib.Authenticate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmaAuthorizationService.Services.Interfaces
{
    public interface IAuthenticateService
    {
        AuthenticateResponse Login(AuthenticateRequest loginInfo);
    }
}
