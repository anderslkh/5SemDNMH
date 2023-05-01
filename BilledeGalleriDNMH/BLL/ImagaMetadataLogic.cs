using Microsoft.AspNetCore.Http;
using Models;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace BLL
{
    public class ImagaMetadataLogic
    {
        public async Task<byte[]> IFormFileToByte(IFormFile file)
        {
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            return fileBytes;
        }

        public Metadata ExtractMetaDataFromByte(byte[] fileBytes)
        {
            var memoryStream = new MemoryStream(fileBytes);

            using (var image = Image.FromStream(memoryStream))
            {
                var metadata = new Metadata
                {
                    Title = GetTitleFromImage(image) ?? "",
                    Description = GetDescriptionFromImage(image) ?? "",
                    DateTime = GetDateTakenFromImage(image) ?? DateTime.Now,
                    Location = GetLocationFromImage(image) ?? "",
                    CameraInformation = GetCameraInformationFromImage(image) ?? "",
                    CopyrightInformation = GetCopyrightInformationFromImage(image) ?? "",
                    Keywords = GetKeywordsFromImage(image) ?? new string[] { "" }
                };

            return metadata;
            }
        }

        //PropertyId is the same as EXIF and it stands for "Exchangeable Image File Format".
        //This type of information is formatted according to the TIFF specification, and may be found in JPG, TIFF, PNG, JP2, PGF, MIFF, HDP, PSP and XCF images,
        //as well as many TIFF-based RAW images, and even some AVI and MOV videos.
        //https://exiftool.org/TagNames/EXIF.html
        private static string GetTitleFromImage(Image image)
        {
            try
            {
                return GetPropertyString(image, 0x9c9b);
            }
            catch
            {

                return null;
            }
        }

        private static string GetDescriptionFromImage(Image image)
        {
            try
            {
                return GetPropertyString(image, 0x010e);
            }
            catch
            {
                return null;
            }
        }

        private static DateTime? GetDateTakenFromImage(Image image)
        {
            try
            {
                var propertyItem = image.GetPropertyItem(0x9003);
                var encoding = new ASCIIEncoding();
                var dateTaken = encoding.GetString(propertyItem.Value, 0, propertyItem.Len - 1);
                return DateTime.ParseExact(dateTaken, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        private static string GetLocationFromImage(Image image)
        {
            try
            {
                return GetPropertyString(image, 0x927c);    
            }
            catch
            {
                return null;
            }
        }

        private static string GetCameraInformationFromImage(Image image)
        {
            try
            {
                return GetPropertyString(image, 0xc614);
            }
            catch
            {
                return null;
            }
            
        }

        private static string GetCopyrightInformationFromImage(Image image)
        {
            try
            {
                return GetPropertyString(image, 0x8298);
            }
            catch
            {
                return null;
            }
        }

        private static string[] GetKeywordsFromImage(Image image)
        {
            try
            {
                var propertyItem = image.GetPropertyItem(0x9c9e);
                string[] strings = new string[10];
                if (propertyItem != null && propertyItem.Type == 2)
                {
                    var value = Encoding.UTF8.GetString(propertyItem.Value, 0, propertyItem.Len);

                    strings = value.Split(';');
                }

                return strings;
            }
            catch
            {

                return null;
            }
        }

        // Helper method to get a string property from the image metadata
        private static string GetPropertyString(Image image, int propertyId)
        {
            var propertyItem = image.GetPropertyItem(propertyId);
            if (propertyItem != null && propertyItem.Type == 2)
            {
                return Encoding.UTF8.GetString(propertyItem.Value, 0, propertyItem.Len - 1);
            }
            return null;
        }
    }
}
