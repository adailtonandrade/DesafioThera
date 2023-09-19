using System.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using Identity.ViewModels;

namespace CrossCutting.JWTConfig
{
    public class JWTService
    {
        public string GenerateToken(LoginVM login, List<Claim> userClaims)
        {
            var key = ConfigurationManager.AppSettings["JwtKey"];

            var issuer = ConfigurationManager.AppSettings["JwtIssuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var handler = new JwtSecurityTokenHandler();

            var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userid", "userId")
            };

            permClaims.AddRange(userClaims);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(permClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Audience = issuer,
                Issuer = issuer,
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        public ClaimsPrincipal ValidateToken(string authToken)
        {
            throw new NotImplementedException();
        }
    }
}
