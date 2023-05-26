using Models;
using Newtonsoft.Json;

namespace WebApp.Service
{
    public class GalleryService
    {
        private readonly HttpClient _httpClient;
        private static readonly string restUrl = "https://localhost:7107/Gallery/";

        public GalleryService() 
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> CreateGallery(string name, List<ImageObject> imageObjects)
        {
            _httpClient.BaseAddress = new Uri(restUrl);

            Gallery gallery = new Gallery
            {
                Name = name,
                ImageObjects = imageObjects
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
