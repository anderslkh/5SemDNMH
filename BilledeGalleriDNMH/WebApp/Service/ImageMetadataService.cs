using Models;
//using MongoDB.Bson.Serialization;
//using MongoDB.Bson;
using MongoDB.Driver;
//using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace WebApp.Service
{
    public class ImageMetadataService
    {
        private readonly HttpClient _httpClient;
        private static readonly string restUrl = "https://localhost:7107/imageMetadatas/";

        public ImageMetadataService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> UploadImage(ImageFile imageFile)
        {
            _httpClient.BaseAddress = new Uri(restUrl);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(restUrl, imageFile);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new Exception($"API call failed with status code {response.StatusCode}");
            };
        }

        public async Task<List<ImageMetadata>> GetImageMetadata(
            string? title = null,
            string? description = null,
            DateTime? dateTime = null,
            string? location = null,
            string? copyrightInformation = null,
            string[]? keywords = null,
            string imageIdentifier = null)
        {
            List<ImageMetadata> foundImageMetadata = null;
            string useUrl = GetCustomUrl(
                title, 
                description, 
                dateTime, 
                location, 
                copyrightInformation, 
                keywords,
                imageIdentifier);

            var uri = new Uri(useUrl);
            try
            {
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode) 
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var documents = JsonConvert.DeserializeObject<List<ImageMetadata>>(content); 
                    foundImageMetadata = documents.ToList();
                }
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
            return foundImageMetadata;
        }

        public string GetCustomUrl(
            string? title = null, 
            string? description = null, 
            DateTime? dateTime = null, 
            string? location = null, 
            string? copyrightInformation = null, 
            string[]? keywords = null,
            string imageIdentifier = null)
        {
            string baseUrl = "https://localhost:7107";
            string endpoint = "imageMetadatas";
            string queryString = "?";

            if (!string.IsNullOrEmpty(title))
            {
                queryString += $"Title={Uri.EscapeDataString(title)}&";
            }
            if (!string.IsNullOrEmpty(description))
            {
                queryString += $"Description={Uri.EscapeDataString(description)}&";
            }
            if (dateTime != null)
            {
                queryString += $"DateTime={Uri.EscapeDataString(dateTime.Value.ToString("o"))}&";
            }
            if (!string.IsNullOrEmpty(location))
            {
                queryString += $"Location={Uri.EscapeDataString(location)}&";
            }
            if (!string.IsNullOrEmpty(copyrightInformation))
            {
                queryString += $"CopyrightInformation={Uri.EscapeDataString(copyrightInformation)}&";
            }
            if (keywords != null && keywords.Any())
            {
                string joinedKeywords = string.Join(',', keywords.Select(k => Uri.EscapeDataString(k)));
                queryString += $"Keywords={joinedKeywords}&";
            }
            if (imageIdentifier != null)
            {
                queryString += $"ImageIdentifier={Uri.EscapeDataString(imageIdentifier)}&";

            }

            queryString = queryString.TrimEnd('&');
            string customUrl = $"{baseUrl}/{endpoint}{queryString}";
            return customUrl;
        }
    }
}
