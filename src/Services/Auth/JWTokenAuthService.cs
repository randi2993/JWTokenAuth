using Domain.ViewModels.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Auth
{
    public class JWTokenAuthService
    {
        private readonly IConfiguration _configuration;

        public JWTokenAuthService(IConfiguration config)
        {
            _configuration = config;
        }

        public JWTokenVM GetToken(UserTokenVM request)
        {
            try
            {
                var model = new JWTokenVM
                {
                    Token = GenerateJwtToken(request),
                    RefreshToken = this.GenerateRandomRefreshToken()
                };

                // Build and Save model to DB here

                return model;
            }
            catch (Exception ex)
            {
                // Add some logs here
                throw ex;
            }
        }

        private string GenerateJwtToken(UserTokenVM request)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", request.UserId.ToString()),
                new Claim("roleId", request.RoleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, request.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:JWToken:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddSeconds(double.Parse(_configuration["Auth:JWToken:expireValueSeconds"]));

            var token = new JwtSecurityToken(
                _configuration["Auth:JWToken:issuer"],
                _configuration["Auth:JWToken:issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRandomRefreshToken()
        {
            int bytes = int.Parse(_configuration["Auth:BytesQTY"]);
            var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[bytes];
            rng.GetBytes(randomBytes); // asign random bytes

            string encode = Convert.ToBase64String(randomBytes);
            return encode;
        }
    }
}
