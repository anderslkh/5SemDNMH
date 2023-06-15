using Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApp.Service
{
    public class ImageMetadataService
    {
        private static readonly string restUrl = "https://localhost:7107/imageMetadatas/";
        private readonly IHttpContextAccessor _contextAccessor;

        public ImageMetadataService(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadImage(ImageMetadata imageMetadata)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var token = _contextAccessor.HttpContext.Request.Cookies["X-Access-Token"];
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                httpClient.BaseAddress = new Uri(restUrl);

                try
                {
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(restUrl, imageMetadata);

                    string result = await response.Content.ReadAsStringAsync();

                    return result;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<List<ImageMetadata>> GetImagesFromIds(string imageIds)
        {
            var url = $"https://localhost:7107/imageMetadatasFromId?imageIds={imageIds}";

            using (HttpClient httpClient = new HttpClient()) 
            {
                var token = _contextAccessor.HttpContext.Request.Cookies["X-Access-Token"];
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                httpClient.BaseAddress = new Uri(restUrl);

                try
                {
                    var response = await httpClient.GetAsync(url);
                    
                    var content = await response.Content.ReadAsStringAsync();

                    var documents = JsonConvert.DeserializeObject<List<ImageMetadata>>(content);

                    return documents;
                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
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


            using (HttpClient httpClient = new HttpClient()) 
            {
                var token = _contextAccessor.HttpContext.Request.Cookies["X-Access-Token"];
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri(useUrl);
                try
                {
                    var response = await httpClient.GetAsync(uri);
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
