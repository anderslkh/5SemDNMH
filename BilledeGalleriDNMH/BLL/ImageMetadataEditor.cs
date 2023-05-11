using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ImageMetadataEditor
    {
        static void Main()
        {
            // Load the image
            Image image = Image.FromFile("path/to/image.jpg");

            // Set the desired EXIF data
            SetPropertyItemString(image, 0x9c9b, "Title", Encoding.UTF8.GetBytes("New image title"));
            SetPropertyItemString(image, 0x010e, "Image Description", Encoding.UTF8.GetBytes("New image description"));
            SetPropertyItemString(image, 0x9003, "DateTime", Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss")));
            SetPropertyItemString(image, 0x8298, "Copyright Information", Encoding.UTF8.GetBytes("New image copyright"));
            SetPropertyItemString(image, 0x9C9E, "Keywords", Encoding.UTF8.GetBytes("New image keywords"));

            // Save the modified image with the updated EXIF data
            image.Save("path/to/image.jpg", ImageFormat.Jpeg);
        }

        // Helper method to set an EXIF property item string value
        private static void SetPropertyItemString(Image image, int propertyId, string propertyName, byte[] propertyValue)
        {
            // Create the new property item
            PropertyItem item = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            item.Id = propertyId;
            item.Type = 2; // ASCII string
            item.Len = propertyValue.Length + 1; // Length of value + null terminator
            item.Value = new byte[propertyValue.Length + 1];
            Array.Copy(propertyValue, item.Value, propertyValue.Length);
            item.Value[propertyValue.Length] = 0; // Add null terminator

            // Set the property item value and update the image's EXIF data
            image.SetPropertyItem(item);
        }
    }
}
