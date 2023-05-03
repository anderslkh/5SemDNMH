using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ImageFormFile
    {
        public string Title { get; set; }
        public IFormFile File { get; set; }
        public string CopyrightInformation { get; set; }
        public string[] Keywords { get; set; }
    }
}
