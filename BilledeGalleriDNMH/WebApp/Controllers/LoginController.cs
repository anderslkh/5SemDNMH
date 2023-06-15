using Microsoft.AspNetCore.Mvc;
using Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("Login");
        }

        public IActionResult Signup()
        {
            return View("SignUp");
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user) 
        {
            LoginService loginService = new LoginService();


            try
            {
                var token = await loginService.Login(user);

                if (token == "Invalid credentials.")
                {
                    return View("Login");
                }

                var cookieOptions = new CookieOptions
                {
                    // Set the cookie expiration time
                    Expires = DateTime.Now.AddHours(1), // Example: token expires after 7 days
                    // Set the cookie to be accessible only through HTTP requests (not JavaScript)
                    HttpOnly = true,
                    // Secure the cookie if using HTTPS
                    Secure = Request.IsHttps
                };

                // Set the JWT token in the cookie
                Response.Cookies.Append("X-Access-Token", token, cookieOptions);
                
                return LocalRedirect("~/Home/Index");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
