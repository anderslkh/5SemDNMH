using Microsoft.AspNetCore.Http;

namespace Models
{
    public class ImageFile
    {
        public string Title { get; set; }
        public byte[] ImageByte { get; set; }
        public string CopyrightInformation { get; set; }
        public string[] Keywords { get; set; }
    }
}
