using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using System.Drawing;
using WebApp.Helpers;
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

        public IActionResult Gallery()
        {
            return View();
        }

        /// <summary>
        /// Gets ImageMetadata based on search parameters, 
        /// adds all found ImageObjects to a list that is returned with view
        /// </summary>
        /// <param name="title">Search by image title</param>
        /// <param name="description">Search by image description</param>
        /// <param name="dateTime">Search by image dateTime</param>
        /// <param name="location">Search by image location</param>
        /// <param name="copyrightInformation">Search by image copyright information</param>
        /// <param name="keywords">Search by keywords on images</param>
        /// <returns>Returns a view with the found ImageObjects</returns>
        [HttpGet]
        public async Task<IActionResult> Images(string title, string description, string dateTime, string location, string copyrightInformation, string keywords)
        {
            string[] keywordArray = Converters.ConvertKeywordsToArray(keywords);
            DateTime? dateTimeValue = Converters.ConvertStringToDateTime(dateTime);

            List<ImageMetadata> images = await _imageMetadataService.GetImageMetadata(title, description, dateTimeValue, location, copyrightInformation, keywordArray);

            List<ImageObject> imageObjects = new List<ImageObject>();

            foreach (var image in images)
            {
                imageObjects.Add(Converters.ConvertBytesToImage(image.Image, image.Title, image.Description, image.ImageIdentifier));
            }

            return View("Images", imageObjects);
        }

        public async Task<IActionResult> MoveToGallery(string[] selectedImages, string title, string description, string dateTime, string location, string copyrightInformation, string keywords)
        {
            string[] keywordArray = Converters.ConvertKeywordsToArray(keywords);
            DateTime? dateTimeValue = Converters.ConvertStringToDateTime(dateTime);

            List<ImageObject> galleryImages = new List<ImageObject>();

            foreach (string imageId in selectedImages)
            {
                var imageMetadatas = await _imageMetadataService.GetImageMetadata(title, description, dateTimeValue, location, copyrightInformation, keywordArray, imageId);
                var imageMetadata = imageMetadatas.First();

                ImageObject imageObject = Converters.ConvertBytesToImage(imageMetadata.Image, imageMetadata.Title, imageMetadata.Description, imageMetadata.ImageIdentifier);
                galleryImages.Add(imageObject);
            }

            return View("GalleryEmbedTest", galleryImages);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
   }
}