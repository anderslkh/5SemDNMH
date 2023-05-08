using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Service;
using static System.Net.Mime.MediaTypeNames;

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
            return View("index");
        }

        [HttpGet]
        [Route("[controller]/Gallery")]
        public async Task<IActionResult> Images(string title, string description, string dateTime, string location, string cameraInformation, string copyrightInformation, string keywords)
        {
            ImageMetadataService metadataService = new();

            string[] keywordArray = ConvertKeywordsToArray(keywords);
            DateTime? dateTimeValue = ConvertStringToDateTime(dateTime);

            List<ImageMetadata> images = await metadataService.GetImageMetadata(title, description, dateTimeValue, location, cameraInformation, copyrightInformation, keywordArray);

            List<ImageObject> imageObjects = new List<ImageObject>();

            foreach (var image in images)
            {
                imageObjects.Add(ConvertBytesToImage(image.Image));
            }

            return View(imageObjects);
        }
        private ImageObject ConvertBytesToImage(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            else
            {
                using (var ms = new MemoryStream(data))
                {
                    var image = System.Drawing.Image.FromStream(ms);

                    return new ImageObject
                    {
                        Data = data,
                        Base64 = Convert.ToBase64String(data),
                        Object = image
                    };
                }
            }
        }

        private string[] ConvertKeywordsToArray(string keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return new string[0];
            }
            else
            {
                return keywords.Split(',').Select(k => k.Trim()).ToArray();
            }
        }

        private DateTime? ConvertStringToDateTime(string dateTimeString)
        {
            if (string.IsNullOrWhiteSpace(dateTimeString))
            {
                return null;
            }
            else if (DateTime.TryParse(dateTimeString, out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                return null;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
   }
}