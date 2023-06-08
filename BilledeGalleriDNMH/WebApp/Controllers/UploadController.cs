using Microsoft.AspNetCore.Mvc;
using Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class UploadController : Controller
    {
        private readonly ImageMetadataService _imageMetadataService;

        public UploadController(ImageMetadataService imageMetadataService)
        {
            _imageMetadataService = imageMetadataService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("[controller]/UploadAsync")]
        public async Task<ActionResult> UploadAsync(ImageFormFile imageFile)
        {
            try
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

                    await _imageMetadataService.UploadImage(imageMetadata);
                }

                return View("index");
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
