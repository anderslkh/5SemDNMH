using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Text;

namespace BLL
{
    public class ImageMetadataEditor
    {
        public static byte[] UpdateExifMetadata(byte[] imageBytes, string imageTitle, string imageDesc, string copyrightInfo, string[] keywords)
        {
            // Load the image from the byte array
            Image image;
            using MemoryStream ms = new MemoryStream(imageBytes);
            image = Image.FromStream(ms);

            // Set the desired EXIF data
            SetPropertyItemString(image, 0x9C9B, "Title", Encoding.UTF8.GetBytes(imageTitle)); //Byte
            SetPropertyItemString(image, 0x010E, "Image Description", Encoding.UTF8.GetBytes(imageDesc)); //Ascii
            SetPropertyItemString(image, 0x8298, "Copyright Information", Encoding.UTF8.GetBytes(copyrightInfo)); //Ascii
            SetPropertyItemString(image, 0x9C9E, "Keywords", Encoding.UTF8.GetBytes(string.Join(", ", keywords))); //Byte

            // Convert the modified image back to a byte array and return it
            using (MemoryStream outputMs = new MemoryStream())
            {
                try
                {
                    image.Save(outputMs, ImageFormat.Jpeg);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                var res = outputMs.ToArray();
                return res;
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
