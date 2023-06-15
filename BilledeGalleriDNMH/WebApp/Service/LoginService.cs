using Models;

namespace WebApp.Service
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private static readonly string restUrl = "https://localhost:7107/login/";

        public LoginService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> Login(User user)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(restUrl, user);
                response.EnsureSuccessStatusCode();

                string token = await response.Content.ReadAsStringAsync();
                return token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
