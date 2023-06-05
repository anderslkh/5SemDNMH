using Microsoft.AspNetCore.Mvc;
using Models;
using WebApp.Helpers;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly GalleryService _galleryService;
        private readonly ImageMetadataService _imageMetadataService;

        public GalleryController(
            ILogger<GalleryController> logger,
            GalleryService galleryService,
            ImageMetadataService imageMetadataService)
        {
            _logger = logger;
            _galleryService = galleryService;
            _imageMetadataService = imageMetadataService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("[controller]/CreateGallery")]
        public async Task<ActionResult> CreateGallery(string galleryName, string imageIdsSingleString)
        {
            List<string> imageIds = imageIdsSingleString.Split(',').ToList();

            if (galleryName != null && imageIdsSingleString != null)
            {
                await _galleryService.CreateGallery(galleryName, imageIds);
            }

            return RedirectToAction("Galleries");
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

            return View("CreateGallery", galleryImages);
        }

        [HttpGet]
        [Route("[controller]/Gallery/")]
        public async Task<IActionResult> Gallery(string imageIds)
        {
            List<ImageMetadata> imageMetadatas = await _imageMetadataService.GetImagesFromIds(imageIds);

            List<ImageObject> imageObjects = new List<ImageObject>();

            foreach (var image in imageMetadatas)
            {
                imageObjects.Add(Converters.ConvertBytesToImage(image.Image, image.Title, image.Description, image.ImageIdentifier));
            }

            return View(imageObjects);
        }

        [HttpGet]
        public async Task<IActionResult> Galleries()
        {
            List<Gallery> galleries = await _galleryService.ReadMany();

            return View(galleries);
        }
    }
}
