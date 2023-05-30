using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
