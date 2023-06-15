using Microsoft.Extensions.Configuration;

namespace Models
{
    public class Configuration
    {
        private static readonly IConfiguration _configuration;

        static Configuration()
        {
            _configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();
        }

        public static string GetConnectionUri()
        {
            return _configuration.GetConnectionString("ConnectionURI");
        }

        public static string GetDatabaseName()
        {
            return _configuration.GetConnectionString("DatabaseName");
        }
        public static string GetSecretKey()
        {
            return _configuration.GetSection("JwtConfig")["SecretKey"];
        }

        public static string GetIssuer()
        {
            return _configuration.GetSection("JwtConfig")["Issuer"];
        }

        public static string GetAudience()
        {
            return _configuration.GetSection("JwtConfig")["Audience"];
        }
    }
}
