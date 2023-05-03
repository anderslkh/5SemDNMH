using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult Upload(ImageFormFile imageFile)
        {
            if (imageFile.File != null && imageFile.File.Length > 0)
            {
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    imageFile.File.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }

            // redirect to the main page
            return RedirectToAction("Index");
        }

        public ActionResult Gallery()
        {
            string[] filenames = Directory.GetFiles(Server.MapPath("~/images"));
            List<string> imageUrls = new List<string>();
            foreach (string filename in filenames)
            {
                string imageUrl = Url.Content("~/images/" + Path.GetFileName(filename));
                imageUrls.Add(imageUrl);
            }

            ViewBag.ImageUrls = imageUrls;
            return View();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}