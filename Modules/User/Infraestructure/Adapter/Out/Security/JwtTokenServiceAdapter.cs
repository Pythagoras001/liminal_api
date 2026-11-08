using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using liminal_api.Modules.User.Application.Port.Out;
using liminal_api.Modules.User.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace liminal_api.Modules.User.Infraestructure.Adapter.Out.Security
{
    internal class JwtTokenServiceAdapter : ITokenServicePort
    {
        private readonly IConfiguration _configuration;

        public JwtTokenServiceAdapter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string generateToken(UserRegister user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}