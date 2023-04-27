using Microsoft.AspNetCore.Mvc;
using Models.Repos;

namespace API.Controllers
{
    [ApiController]
    [Route("/controller")]
    public class ImageMetadataController : Controller
    {
        [HttpGet]
        public bool TestConnection()
        {
            var res = ImageMetadataRepo.GetMongoDBConnection();
            return res;
        }
    }


}
