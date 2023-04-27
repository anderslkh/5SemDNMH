using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;
using MongoDBRepository.Repositories;
using Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MongoDBRepository.Repository
{
    public class ImageMetadataRepository : GenericRepository<ImageMetadata>
    {
        public ImageMetadataRepository(IOptions<MongoDBSettings> mongoDBSettings) : base(mongoDBSettings) { }

        protected override FilterDefinition<ImageMetadata> CreateFilterDefinition(BaseQueryParameters? filterObject)
        {
            
        }
    }
}
