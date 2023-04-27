using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Metadata
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }
        public string CameraInformation { get; set; }
        public string CopyrightInformation { get; set; }
        public List<string> Keywords { get; set; }
    }
}
