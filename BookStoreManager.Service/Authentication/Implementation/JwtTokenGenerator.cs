using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStoreManager.Service.Authentication.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace BookStoreManager.Service.Authentication.Implementation
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _JwtSettings;

        public JwtTokenGenerator( IOptions<JwtSettings> jwtSettings)
        {
            _JwtSettings = jwtSettings.Value;
        }

        public string GeneratedToken(Guid userId, string firstname, string lastname)
        {
            SigningCredentials signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_JwtSettings.Secrets)),
                SecurityAlgorithms.HmacSha256
            );
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, firstname),
                new Claim(JwtRegisteredClaimNames.FamilyName, lastname),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var securityToken = new JwtSecurityToken(
                issuer: _JwtSettings.Issuer,
                audience: _JwtSettings.Audience,
                expires: DateTime.Now.AddHours(_JwtSettings.ExpiryInHour),
                claims: claims,
                 signingCredentials: signingCredentials);

             return new JwtSecurityTokenHandler().WriteToken(securityToken);    
        }
    }
}