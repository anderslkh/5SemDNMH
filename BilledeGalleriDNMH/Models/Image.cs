using Microsoft.AspNetCore.Http;

namespace Models
{
    public class Image
    {
        public string Title { get; set; }
        public IFormFile ImageFile { get; set; }
        public string CopyrightInformation { get; set; }
        public string[] Keywords { get; set; }
    }
}
