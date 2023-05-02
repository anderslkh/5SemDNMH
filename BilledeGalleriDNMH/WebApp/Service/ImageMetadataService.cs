using Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;

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

        public async Task<List<ImageMetadata>> GetImageMetadata(
            string? title = null,
            string? description = null,
            DateTime? dateTime = null,
            string? location = null,
            string? cameraInformation = null,
            string? copyrightInformation = null,
            string[]? keywords = null)
        {
            List<ImageMetadata> foundImageMetadata = null;
            string useUrl = GetCustomUrl(
                title, 
                description, 
                dateTime, 
                location, 
                cameraInformation, 
                copyrightInformation, 
                keywords);

            var uri = new Uri(useUrl);
            try
            {
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode) 
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var documents = BsonSerializer.Deserialize<List<ImageMetadata>>(content);
                    foundImageMetadata = documents.ToList();
                    //foundImageMetadata = JsonConvert.DeserializeObject<List<ImageMetadata>>(content).ToList();
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
            string? cameraInformation = null, 
            string? copyrightInformation = null, 
            string[]? keywords = null)
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
            if (!string.IsNullOrEmpty(cameraInformation))
            {
                queryString += $"CameraInformation={Uri.EscapeDataString(cameraInformation)}&";
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

            queryString = queryString.TrimEnd('&');
            string customUrl = $"{baseUrl}/{endpoint}{queryString}";
            return customUrl;
        }
    }
}
