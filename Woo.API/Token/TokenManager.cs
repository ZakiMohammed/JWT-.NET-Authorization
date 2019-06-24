using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace Woo.API
{
    public class TokenManager
    {
        private static readonly string secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                {
                    return null;
                }

                var key = Convert.FromBase64String(secret);
                var parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),                    
                    ValidateLifetime = true,
                    LifetimeValidator = LifetimeValidator,
                };

                return tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
            }
            catch
            {
                return null;
            }
        }

        public static string GenerateToken(string username)
        {
            var key = Convert.FromBase64String(secret);
            var securityKey = new SymmetricSecurityKey(key);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public static string ValidateToken(string token)
        {            
            var principal = GetPrincipal(token);
            if (principal == null)
            {
                return null;
            }

            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity) principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);            
            return usernameClaim.Value;
        }

        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

        public static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }
    }
}