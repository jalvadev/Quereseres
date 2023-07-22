using API.Quereseres.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Quereseres.Helpers
{
    public static class JWTHelper
    {
        public static string GenerateToken(User user, string key, string issuer, string audience)
        {
            var jwtKey = Encoding.UTF8.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.Name),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }

        public static int GetUserId(HttpContext context)
        {
            int userId;

            try
            {
                var user = context.User;
                string userIdString = user.FindFirstValue("Id");
                userId = Convert.ToInt32(userIdString);

            }catch(Exception ex)
            {
                userId = -1;
            }

            return userId;
        }
    }
}
