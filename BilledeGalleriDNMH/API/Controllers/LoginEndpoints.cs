using BLL;

namespace API.Controllers
{
    public static class LoginEndpoints
    {
        public static void AddLoginEndpoints(this WebApplication application)
        {
            application.MapPost("/login", Login);
        }

        static async Task<string> Login(string email, string password)
        {
            JWTLogic jWTLogic = new();

            try
            {
                var tokenString = await jWTLogic.GenerateJwt(email, password);
                return tokenString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
