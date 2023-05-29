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

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Route("[controller]/Gallery/{galleryname}")]
        public async Task<IActionResult> ReadOne(string galleryName)
        {
            Gallery gallery = await _galleryService.ReadOne(galleryName);



            var name = gallery.GalleryName;
            List<string> imageIds = gallery.ImageIds;

            return View("Gallery", imageIds);
        }

        [HttpGet]
        public async Task<IActionResult> GetImagesFromIds(string imageIds)
        {
            List<ImageMetadata> imageMetadatas = await _imageMetadataService.GetImagesFromIds(imageIds);

            List<ImageObject> imageObjects = new List<ImageObject>();

            foreach (var image in imageMetadatas)
            {
                imageObjects.Add(Converters.ConvertBytesToImage(image.Image, image.Title, image.Description, image.ImageIdentifier));
            }
             //todo returner korrekt view
            return null;

        }

        [HttpGet]
        public async Task<IActionResult> Galleries()
        {
            List<Gallery> galleries = await _galleryService.ReadMany();

            return View(galleries);
        }

    }
}
