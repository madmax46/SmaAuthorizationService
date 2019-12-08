using AuthCommonLib.Authorize;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmaAuthorizationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SmaAuthorizationService.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly TokenValidationParameters tokenValidationParameters;
        public AuthorizationService()
        {
            tokenHandler = new JwtSecurityTokenHandler();

            tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = AuthOptions.ISSUER,
                ValidAudience = AuthOptions.AUDIENCE,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
            };
        }
        public AuthorizeTokenResponse CheckToken(AuthorizeTokenRequest request)
        {
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(request.Token, tokenValidationParameters, out var validatedToken);
                var encodedJwt = tokenHandler.ReadJwtToken(request.Token);

                var role = encodedJwt.Claims.FirstOrDefault(r => r.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
                var userInfo = new AuthCommonLib.UserInfo()
                {
                    Email = encodedJwt.Claims.FirstOrDefault(r => r.Type == "Email")?.Value,
                    FirstName = encodedJwt.Claims.FirstOrDefault(r => r.Type == "FirstName")?.Value,
                    SecondName = encodedJwt.Claims.FirstOrDefault(r => r.Type == "SecondName")?.Value,
                    UserRoles = new List<string>() { role }
                };

                return new AuthorizeTokenResponse()
                {
                    IsSuccess = true,
                    User = userInfo
                };
            }
            catch (Exception ex)
            {
                return new AuthorizeTokenResponse()
                {
                };
            }
        }
    }
}
