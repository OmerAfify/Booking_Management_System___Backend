using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.IServices;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public string CreateToken(ApplicationUser applicationUser)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaimsAsync(applicationUser);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims.Result);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration["Jwt:Key"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var roles =  await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName)
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(

                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddDays(Convert.ToDouble((_configuration.GetSection("Jwt")).GetSection("Lifetime").Value))
                );

            return tokenOptions;
        }


    }
}
