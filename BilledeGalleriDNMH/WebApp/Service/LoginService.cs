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

        public async Task<string> Login(string email, string password)
        {
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("password", password)
            });

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(restUrl, formData);
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
