using AuthCommonLib;
using AuthCommonLib.Authenticate;
using Microsoft.IdentityModel.Tokens;
using SmaAuthorizationService.Repositories.Interfaces;
using SmaAuthorizationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmaAuthorizationService.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IAuthenticateRepository authenticateRepository;

        public AuthenticateService(IAuthenticateRepository authenticateRepo)
        {
            authenticateRepository = authenticateRepo;
        }

        public AuthenticateResponse Login(AuthenticateRequest loginInfo)
        {
            var username = loginInfo.Login;
            var password = loginInfo.Password;

            var (identity, person) = GetIdentity(username, password);
            if (identity == null || person == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    //expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var userInfo = new UserInfo(person.FirstName, person.SecondName, person.EMail, new List<string>() { person.Role });
            return new AuthenticateResponse(true, encodedJwt, username, userInfo);

        }


        private (ClaimsIdentity identity, Person person) GetIdentity(string username, string password)
        {
            Person person = authenticateRepository.GetUserByLoginAndPassword(username, password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role),
                    new Claim("FirstName", person.FirstName),
                    new Claim("SecondName", person.SecondName),
                    new Claim("Email", person.EMail)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    person.Role);
                return (claimsIdentity, person);
            }

            // если пользователя не найдено
            return (null, null);
        }

    }
}
