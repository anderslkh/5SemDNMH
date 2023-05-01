using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;

namespace MongoDBRepository.Repositories
{
    public class ImageMetadataRepository
    {
        protected IMongoCollection<ImageMetadata> collection;

        public ImageMetadataRepository()
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(Configuration.GetConnectionUri());

            MongoClient client = new MongoClient(Configuration.GetConnectionUri());

            IMongoDatabase database = client.GetDatabase(Configuration.GetDatabaseName());

            collection = database.GetCollection<ImageMetadata>(typeof(ImageMetadata).Name);
        }

        public async Task Create(ImageMetadata imageMetadata)
        {
            await collection.InsertOneAsync(imageMetadata);
        }

        public async Task<bool> Delete(Guid id)
        {
            FilterDefinition<ImageMetadata> filter = Builders<ImageMetadata>.Filter.Eq(T => T.Id, id);
            DeleteResult result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount != 0;
        }

        public async Task<List<ImageMetadata>> ReadMany(BaseQueryParameters? filterObject = null)
        {
            FilterDefinition<ImageMetadata> filter = CreateFilterDefinition(filterObject);

            SortDefinition<ImageMetadata> sort = CreateSortDefinition(filterObject);

            List<ImageMetadata> list = await collection.Find(filter).Sort(sort).ToListAsync();

            return list;
        }

        protected virtual FilterDefinition<ImageMetadata> CreateFilterDefinition(BaseQueryParameters? filterObject)
        {
            FilterDefinition<ImageMetadata> filter = Builders<ImageMetadata>.Filter.Empty;

            if (filterObject is not null)
            {
                Console.Error.WriteLine("Filtering is not supported on generic repositories");
            }

            return filter;
        }

        protected virtual SortDefinition<ImageMetadata> CreateSortDefinition(BaseQueryParameters? sortObject) 
        {
            SortDefinitionBuilder<ImageMetadata> builder = Builders<ImageMetadata>.Sort;

            var sort = builder.Ascending(T => T.Id);

            return sort;
        }
    }
}
