using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Service;

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

        [HttpPost]
        [Route("[controller]/UploadAsync")]
        public async Task<ActionResult> UploadAsync(ImageFormFile imageFile)
        {
            if (imageFile.File != null)
            {
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    imageFile.File.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();

                    ImageFile file = new ImageFile
                    {
                        CopyrightInformation = imageFile.CopyrightInformation,
                        ImageByte = imageBytes,
                        Keywords = imageFile.Keywords,
                        Title = imageFile.Title
                    };

                    ImageMetadataService imageMetadataService = new();

                    await imageMetadataService.UploadImage(file);
                }
            }
            return View("Succes");
        }

        //public ActionResult Gallery()
        //{
        //    string[] filenames = Directory.GetFiles(Server.MapPath("~/images"));
        //    List<string> imageUrls = new List<string>();
        //    foreach (string filename in filenames)
        //    {
        //        string imageUrl = Url.Content("~/images/" + Path.GetFileName(filename));
        //        imageUrls.Add(imageUrl);
        //    }

        //    ViewBag.ImageUrls = imageUrls;
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
   }
}