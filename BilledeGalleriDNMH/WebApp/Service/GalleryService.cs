using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Http;
using Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace WebApp.Service
{
    public class GalleryService
    {
        private readonly HttpClient _httpClient;
        private static readonly string restUrl = "https://localhost:7107/Gallery/";
        private readonly IHttpContextAccessor _contextAccessor;

        public GalleryService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpClient = new HttpClient();
            _contextAccessor = httpContextAccessor;

            var token = _contextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> CreateGallery(string galleryName, List<string> imageIds)
        {
            _httpClient.BaseAddress = new Uri(restUrl);

            Gallery gallery = new Gallery
            {
                GalleryName = galleryName,
                ImageIds = imageIds
            };

            var response = await _httpClient.PostAsJsonAsync(restUrl, gallery);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new Exception($"API call failed with status code {response.StatusCode}");
            }
        }

        public async Task<Gallery> ReadOne(string galleryName)
        {
            var url = $"https://localhost:7107/Gallery&name={galleryName}";
            Gallery gallery = null;

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode) 
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var documents = JsonConvert.DeserializeObject<Gallery>(content);
                    gallery = documents;
                }
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
            return gallery;
        }

        public async Task<List<Gallery>> ReadMany()
        {
            var url = $"https://localhost:7107/Galleries";
            List<Gallery> galleries = null;

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var documents = JsonConvert.DeserializeObject<List<Gallery>>(content);
                    galleries = documents;
                }
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
            return galleries;
        }
    }
}
