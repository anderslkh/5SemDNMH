using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDBRepository.Repositories;

namespace API.Controllers
{
    public static class ImageMetadataEndpoints
    {
        public static void AddImageMetaDataEndpoints(this WebApplication application)
        {
            application.MapPost("/imageMetadatas", Create);
            application.MapGet("/imageMetadatas", GetMany);
            application.MapGet("/imageMetadatasFromId", GetManyFromId);
        }

        [Authorize]
        static async Task<IResult> Create([FromBody] ImageMetadata imageMetadata)
        {
            ImageMetadataRepository imageMetadataRepository = new ImageMetadataRepository();
            try
            {
                var imageUpdated = ImageMetadataEditor.UpdateExifMetadata(imageMetadata.Image, imageMetadata.Title,
                imageMetadata.Description, imageMetadata.CopyrightInformation, imageMetadata.Keywords);

                imageMetadata.Image = imageUpdated;
                imageMetadata.ImageIdentifier = Guid.NewGuid().ToString();

                await imageMetadataRepository.Create(imageMetadata);

                return Results.Ok();
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Authorize]
        static async Task<List<ImageMetadata>> GetMany([AsParameters] ImageMetadataQueryParameters imageMetadataQuery)
        {
            ImageMetadataRepository imageMetadataRepository = new();

            try
            {
                List<ImageMetadata> result = await imageMetadataRepository.ReadMany(imageMetadataQuery);

                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Authorize]
        static async Task<List<ImageMetadata>> GetManyFromId(string imageIds)
        {
            ImageMetadataRepository imageMetadataRepository = new();
            try
            {
                List<ImageMetadata> result = await imageMetadataRepository.ReadManyFromId(imageIds);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
