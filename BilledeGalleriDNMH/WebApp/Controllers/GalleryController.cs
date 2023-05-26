using Microsoft.AspNetCore.Mvc;
using Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly GalleryService _galleryService;

        public GalleryController(
            ILogger<GalleryController> logger,
            GalleryService galleryService)
        {
            _logger = logger;
            _galleryService = galleryService;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("[controller]/CreateGallery")]
        public async Task<ActionResult> CreateGallery(string galleryName, List<ImageObject> imageObjects)
        {

            if (galleryName != null && imageObjects != null) 
            {
                await _galleryService.CreateGallery(galleryName, imageObjects);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Route("[controller]/Gallery/{galleryname}")]
        public async Task<IActionResult> ReadOne(string galleryName)
        {
            Gallery gallery = await _galleryService.ReadOne(galleryName);

            var name = gallery.Name;
            List<ImageObject> imageObjects = gallery.ImageObjects;

            return View("Gallery", imageObjects);
        }

        [HttpGet]
        public async Task<IActionResult> Galleries()
        {
            List<Gallery> galleries = await _galleryService.ReadMany();

            return View(galleries);
        }

    }
}
