using Microsoft.AspNetCore.Mvc;
using Models;
using WebApp.Helpers;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ImageMetadataService _imageMetadataService;

        public ImagesController(ImageMetadataService imageMetadataService)
        {
            _imageMetadataService = imageMetadataService;
        }


        public IActionResult Index()
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

            return View(imageObjects);
        }
    }
}
