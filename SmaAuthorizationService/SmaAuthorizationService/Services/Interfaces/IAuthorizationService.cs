using AuthCommonLib.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmaAuthorizationService.Services.Interfaces
{
    public interface IAuthorizationService
    {
        AuthorizeTokenResponse CheckToken(AuthorizeTokenRequest request);
    }
}
