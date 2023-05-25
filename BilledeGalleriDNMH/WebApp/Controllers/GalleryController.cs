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
        [Route("[controller]CreateGallery")]
        public async Task<ActionResult> CreateGallery(string name, List<ImageObject> imageObjects)
        {

            if (name != null && imageObjects != null) 
            {
                await _galleryService.CreateGallery(name, imageObjects);
            }

            return View("index");
        }
    }
}
