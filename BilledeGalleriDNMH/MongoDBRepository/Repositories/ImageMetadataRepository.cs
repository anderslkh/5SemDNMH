using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Net.Mime.MediaTypeNames;
using System;
using MongoDB.Bson;

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

        public async Task<List<ImageMetadata>> FilterImageMetadata(ImageMetadataQueryParameters queryParameters)
        {

            // create a list of possible filters
            var filters = new List<FilterDefinition<ImageMetadata>>();

            // check if the "Image" property has a value, and add a filter if it does
            if (queryParameters.Image != null && queryParameters.Image.Length > 0)
            {
                filters.Add(Builders<ImageMetadata>.Filter.Eq("Image", queryParameters.Image));
            }

            // check if the "Title" property has a value, and add a filter if it does
            if (!string.IsNullOrEmpty(queryParameters.Title))
            {
                filters.Add(Builders<ImageMetadata>.Filter.Eq("Title", queryParameters.Title));
            }

            // check if the "Description" property has a value, and add a filter if it does
            if (!string.IsNullOrEmpty(queryParameters.Description))
            {
                filters.Add(Builders<ImageMetadata>.Filter.Eq("Description", queryParameters.Description));
            }

            // check if the "DateTime" property has a value, and add a filter if it does
            if (queryParameters.DateTime.HasValue)
            {
                filters.Add(Builders<ImageMetadata>.Filter.Eq("DateTime", queryParameters.DateTime.Value));
            }

            // check if the "Location" property has a value, and add a filter if it does
            if (!string.IsNullOrEmpty(queryParameters.Location))
            {
                filters.Add(Builders<ImageMetadata>.Filter.Eq("Location", queryParameters.Location));
            }

            // check if the "CameraInformation" property has a value, and add a filter if it does
            if (!string.IsNullOrEmpty(queryParameters.CameraInformation))
            {
                filters.Add(Builders<ImageMetadata>.Filter.Eq("CameraInformation", queryParameters.CameraInformation));
            }

            // check if the "CopyrightInformation" property has a value, and add a filter if it does
            if (!string.IsNullOrEmpty(queryParameters.CopyrightInformation))
            {
                filters.Add(Builders<ImageMetadata>.Filter.Eq("CopyrightInformation", queryParameters.CopyrightInformation));
            }

            // check if the "Keywords" property has a value, and add a filter if it does
            if (queryParameters.Keywords != null && queryParameters.Keywords.Length > 0)
            {
                var keywordFilters = queryParameters.Keywords.Select(keyword =>
                    Builders<ImageMetadata>.Filter.Eq("Keywords", keyword)
                );

                var orFilter = Builders<ImageMetadata>.Filter.Or(keywordFilters);

                filters.Add(orFilter);
            }

            // combine the filters using the logical AND operator
            var combinedFilter = Builders<ImageMetadata>.Filter.And(filters);

            // execute the query
            var results = await collection.Find(combinedFilter).ToListAsync();

            return results;
        }

        protected virtual SortDefinition<ImageMetadata> CreateSortDefinition(BaseQueryParameters? sortObject) 
        {
            SortDefinitionBuilder<ImageMetadata> builder = Builders<ImageMetadata>.Sort;

            var sort = builder.Ascending(T => T.Id);

            return sort;
        }

        public FilterDefinition<ImageMetadata> CreateFilterDefinition(BaseQueryParameters? filterObject)
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

                if (queryParameters.Image != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.Image, queryParameters.Image);
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

                if (queryParameters.CameraInformation != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.CameraInformation, queryParameters.CameraInformation);
                }

                if (queryParameters.CopyrightInformation != null)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.CopyrightInformation, queryParameters.CopyrightInformation);
                }

                if (queryParameters.Keywords.Length != 0)
                {
                    filter &= filterBuilder.Eq(imageMetadata => imageMetadata.Keywords, queryParameters.Keywords);
                }
            }

            return filter;
        }
    }
}
