    using Microsoft.Extensions.Options;
    using Models;
    using MongoDB.Driver;

    namespace MongoDBRepository.Repositories
    {
        public class ImageMetadataRepository
        {
            protected IMongoCollection<ImageMetadata> collection;

            public ImageMetadataRepository(IOptions<MongoDBSettings> mongoDBSettings)
            {
                MongoClientSettings settings = MongoClientSettings.FromConnectionString(mongoDBSettings.Value.ConnectionURI);

                MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);

                IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

                collection = database.GetCollection<ImageMetadata>(typeof(ImageMetadata).Name);
            }

            public async Task<ImageMetadata> Create(ImageMetadata imageMetadata)
            {
                await collection.InsertOneAsync(imageMetadata);
                return imageMetadata;
            }

            public async Task<bool> Delete(Guid id)
            {
                FilterDefinition<ImageMetadata> filter = Builders<ImageMetadata>.Filter.Eq(T => T.Id, id);
                DeleteResult result = await collection.DeleteOneAsync(filter);
                return result.DeletedCount != 0;
            }

            public Task<ImageMetadata> Read(ImageMetadata imageMetadata)
            {
                throw new NotImplementedException();
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
        }
    }
