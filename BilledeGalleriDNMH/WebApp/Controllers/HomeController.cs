using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using System.Drawing;
using WebApp.Models;
using WebApp.Service;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ImageMetadataService _imageMetadataService;

        public HomeController(
            ILogger<HomeController> logger, 
            ImageMetadataService imageMetadataService)
        {
            _logger = logger;
            _imageMetadataService = imageMetadataService;
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
            // Save imageBytes to database

            if (imageFile.File != null)
            {
                var filePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await imageFile.File.CopyToAsync(stream);
                }
                var imageBytes = System.IO.File.ReadAllBytes(filePath);

                ImageMetadata imageMetadata = new ImageMetadata 
                { 
                    Image = imageBytes,
                    Title = imageFile.Title,
                    Description = imageFile.Description,
                    DateTime = DateTime.Now,
                    Location = imageFile.Location,
                    CopyrightInformation = imageFile.CopyrightInformation,
                    Keywords = imageFile.Keywords.Split(",").Select(x => x.Trim()).ToArray()
                };

                //ImageMetadataService imageMetadataService = new();

                await _imageMetadataService.UploadImage(imageMetadata);
            }

            return View("index");
        }

        [HttpGet]
        public async Task<IActionResult> Images(string title, string description, string dateTime, string location, string copyrightInformation, string keywords)
        {
            //ImageMetadataService metadataService = new();

            string[] keywordArray = ConvertKeywordsToArray(keywords);
            DateTime? dateTimeValue = ConvertStringToDateTime(dateTime);

            List<ImageMetadata> images = await _imageMetadataService.GetImageMetadata(title, description, dateTimeValue, location, copyrightInformation, keywordArray);

            List<ImageObject> imageObjects = new List<ImageObject>();

            foreach (var image in images)
            {
                imageObjects.Add(ConvertBytesToImage(image.Image, image.Title, image.Description, image.ImageIdentifier));
            }

            return View("Images", imageObjects);
        }

        public async Task<IActionResult> MoveToGallery(string[] selectedImages, string title, string description, string dateTime, string location, string copyrightInformation, string keywords)
        {
            //ImageMetadataService metadataService = new();

            string[] keywordArray = ConvertKeywordsToArray(keywords);
            DateTime? dateTimeValue = ConvertStringToDateTime(dateTime);

            List<ImageObject> galleryImages = new List<ImageObject>();

            foreach (string imageId in selectedImages)
            {
                var imageMetadatas = await _imageMetadataService.GetImageMetadata(title, description, dateTimeValue, location, copyrightInformation, keywordArray, imageId);
                var imageMetadata = imageMetadatas.First();

                ImageObject imageObject = ConvertBytesToImage(imageMetadata.Image, imageMetadata.Title, imageMetadata.Description, imageMetadata.ImageIdentifier);
                galleryImages.Add(imageObject);
            }

            return View("GalleryEmbedTest", galleryImages);
        }

        private ImageObject ConvertBytesToImage(byte[] data, string title, string description, string id)
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
                        Object = image,
                        ImageTitle = title,
                        ImageDescription = description,
                        Id = id,
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