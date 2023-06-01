using BLL;
using Models;

namespace API.Controllers
{
    public static class LoginEndpoints
    {
        public static void AddLoginEndpoints(this WebApplication application)
        {
            application.MapPost("/login", Login);
        }

        static async Task<string> Login(User user)
        {
            JWTLogic jWTLogic = new();

            try
            {
                var tokenString = await jWTLogic.GenerateJwt(user.Email, user.Password);
                return tokenString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
