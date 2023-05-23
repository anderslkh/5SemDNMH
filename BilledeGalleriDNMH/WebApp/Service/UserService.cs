using Models;
using Newtonsoft.Json;

namespace WebApp.Service
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private static readonly string restUrl = "https://localhost:7107/User/";

        public UserService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> CreateUser(string email, string password)
        {
            _httpClient.BaseAddress = new Uri(restUrl);

            User user = new User 
            { 
                Email = email, 
                Password = password 
            };

            var response = await _httpClient.PostAsJsonAsync(restUrl, user);

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

        public async Task<User> ReadOne(string email)
        {
            var url = $"https://localhost:7107/User&email={email}";
            User user = null;

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var documents = JsonConvert.DeserializeObject<User>(content);
                    user = documents;
                }
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }

            return user;

        }
    }
}
