using Models;

namespace WebApp.Helpers
{
    public class Converters
    {
        public static ImageObject ConvertBytesToImage(byte[] data, string title, string description, string id)
        {
            if (data == null)
            {
                return null;
            }
            else
            {
                using (var ms = new MemoryStream(data))
                {
                    var image = System.Drawing.Image.FromStream(ms);

                    return new ImageObject
                    {
                        Data = data,
                        Base64 = Convert.ToBase64String(data),
                        Object = image,
                        ImageTitle = title,
                        ImageDescription = description,
                        Id = id,
                    };
                }
            }
        }

        public static string[] ConvertKeywordsToArray(string keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return new string[0];
            }
            else
            {
                return keywords.Split(',').Select(k => k.Trim()).ToArray();
            }
        }

        public static DateTime? ConvertStringToDateTime(string dateTimeString)
        {
            if (string.IsNullOrWhiteSpace(dateTimeString))
            {
                return null;
            }
            else if (DateTime.TryParse(dateTimeString, out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                return null;
            }
        }
    }
}
