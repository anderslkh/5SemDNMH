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
            FilterDefinition<ImageMetadata> filter = Builders<ImageMetadata>.Filter.Eq(imageMetadata => imageMetadata.Id, id);
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

        public static FilterDefinition<ImageMetadata> CreateFilterDefinition(BaseQueryParameters? filterObject)
        {
            FilterDefinitionBuilder<ImageMetadata> filterBuilder = Builders<ImageMetadata>.Filter;

            FilterDefinition<ImageMetadata> filter = filterBuilder.Empty;

            if (filterObject != null && filterObject is ImageMetadataQueryParameters)
            {
                ImageMetadataQueryParameters queryParameters = (ImageMetadataQueryParameters)filterObject;

                if (queryParameters.Id != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.Id, queryParameters.Id);
                }

                if (queryParameters.Image != null && queryParameters.Image.Length > 0)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.Image, queryParameters.Image);
                }

                if (queryParameters.ImageIdentifier != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.ImageIdentifier, queryParameters.ImageIdentifier);
                }

                if (queryParameters.Title != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.Title, queryParameters.Title);
                }

                if (queryParameters.Description != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.Description, queryParameters.Description);
                }

                if (queryParameters.DateTime != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.DateTime, queryParameters.DateTime);
                }

                if (queryParameters.Location != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.Location, queryParameters.Location);
                }

                if (queryParameters.CopyrightInformation != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.CopyrightInformation, queryParameters.CopyrightInformation);
                }

                if (queryParameters.Keywords != null && queryParameters.Keywords.Length > 0)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.Keywords, queryParameters.Keywords);
                }
            }

            return filter;
        }

        protected virtual SortDefinition<ImageMetadata> CreateSortDefinition(BaseQueryParameters? sortObject)
        {
            SortDefinitionBuilder<ImageMetadata> builder = Builders<ImageMetadata>.Sort;

            var sort = builder.Ascending(imageMetadata => imageMetadata.Id);

            return sort;
        }

    }
}
