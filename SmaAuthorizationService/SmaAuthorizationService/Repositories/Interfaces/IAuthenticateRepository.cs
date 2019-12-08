using AuthCommonLib;
using DbWrapperCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmaAuthorizationService.Repositories.Interfaces
{
    public interface IAuthenticateRepository
    {
        Person GetUserByLoginAndPassword(string login, string password);
    }
}
