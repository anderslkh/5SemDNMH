using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ImageObject
    {
        public string Id { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        public byte[] Data { get; set; }
        public string Base64 { get; set; }
        public System.Drawing.Image Object { get; set; }
    }
}
