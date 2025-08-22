using Microsoft.IdentityModel.Tokens;
using shop.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace shop.Services.JwtService
{
    public class JwtTokenGenerator
    {
        private readonly string _secretKey;
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _secretKey = configuration["AuthSettings:SecretKey"];
        }

        public string GenerateJwtToken(AppUser appUser, IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject = new ClaimsIdentity(new Claim[] { 
                    new Claim("FirstName", appUser.UserName),
                    new Claim("id", appUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, appUser.Email),
                    new Claim(ClaimTypes.Role, String.Join(",", roles))
                }),

                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
