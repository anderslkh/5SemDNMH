using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;
using MongoDBRepository.Repositories;

namespace MongoDBRepository.Repository
{
    public class ImageMetadataFilterDefinition : ImageMetadataRepository
    {
        protected override FilterDefinition<ImageMetadata> CreateFilterDefinition(BaseQueryParameters? filterObject)
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
