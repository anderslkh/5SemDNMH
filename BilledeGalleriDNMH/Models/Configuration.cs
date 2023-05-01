using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
