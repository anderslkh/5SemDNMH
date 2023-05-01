using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace MongoDBRepository
{
    public static class MongoDBSetup
    {
        public static void SetupMongoDBServices(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));


        }
    }
}
