using Microsoft.AspNetCore.Mvc;
using Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SignUp(User user) 
        {
            UserService userService = new UserService(); 
            
            try
            {
                await userService.CreateUser(user);

                return RedirectToAction("Login", "Login");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
