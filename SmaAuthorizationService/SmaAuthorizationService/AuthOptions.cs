using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaAuthorizationService
{
    public class AuthOptions
    {
        public const string ISSUER = "SmaAuthService"; // издатель токена
        public const string AUDIENCE = "http://localhost/"; // потребитель токена
        const string KEY = "supersecret_secretkey!12345";   // ключ для шифрации
        public const int LIFETIME = 0; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }

}
