using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImageMetadataService _imageMetadataService;

        public HomeController(ImageMetadataService imageMetadataService)
        {
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