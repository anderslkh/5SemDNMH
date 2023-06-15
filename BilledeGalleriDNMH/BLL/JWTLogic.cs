using Microsoft.IdentityModel.Tokens;
using Models;
using MongoDBRepository.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL
{
    public class JWTLogic
    {
        private readonly string _secretKey = Configuration.GetSecretKey();
        private readonly string _issuer = Configuration.GetIssuer();
        private readonly string _audience = Configuration.GetAudience();

        public async Task<string> GenerateJwt(string email, string password)
        {
            //var secretKey = _config.GetValue<string>("JwtConfig:SecretKey");
            UserRepository userRepository = new UserRepository();

            User user = await userRepository.ReadOne(email);

            // Verify the user's identity
            if (!PasswordHelper.ComparePass(password, user.Password, user.Salt))
            {
                throw new Exception("Invalid credentials.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public ClaimsIdentity ValidateToken(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.ASCII.GetBytes(_secretKey);
            var keyString = Encoding.ASCII.GetString(keyBytes);
            var key = Convert.FromBase64String(keyString);

            try
            {
                tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "Jwt");
                return claimsIdentity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}