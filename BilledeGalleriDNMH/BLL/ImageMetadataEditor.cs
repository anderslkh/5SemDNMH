using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ImageMetadataEditor
    {
        public static byte[] UpdateExifMetadata(byte[] imageBytes, string imageTitle, string imageDesc, string copyrightInfo, string[] keywords)
        {
            // Load the image from the byte array
            Image image;
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                image = Image.FromStream(ms);
            }

            // Set the desired EXIF data
            SetPropertyItemString(image, 0x9C9B, "Title", Encoding.UTF8.GetBytes("New image title")); //Byte
            SetPropertyItemString(image, 0x010E, "Image Description", Encoding.UTF8.GetBytes("New image description")); //Ascii
            SetPropertyItemString(image, 0x8298, "Copyright Information", Encoding.UTF8.GetBytes("New image copyright")); //Ascii
            SetPropertyItemString(image, 0x9C9E, "Keywords", Encoding.UTF8.GetBytes("New image keywords")); //Byte

            // Modify the "ImageDescription" property data
            PropertyItem imageDescription = image.PropertyItems.FirstOrDefault(p => p.Id == 0x010E);

            if (imageDescription != null)
            {
                imageDescription.Value = Encoding.ASCII.GetBytes("new image description");
                image.SetPropertyItem(imageDescription);
            }

            // Modify the "ImageDescription" property data
            PropertyItem copyrightInformation = image.PropertyItems.FirstOrDefault(p => p.Id == 0x8298);

            if (imageDescription != null)
            {
                copyrightInformation.Value = Encoding.ASCII.GetBytes("new copyright information");
                image.SetPropertyItem(imageDescription);
            }

            // Convert the modified image back to a byte array and return it
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
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
