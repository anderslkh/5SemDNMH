using BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using MongoDBRepository;
using MongoDBRepository.Repositories;
using System;

namespace API.Controllers
{

    public static class ImageMetadataEndpoints
    {
        public static void AddImageMetaDataEndpoints(this WebApplication application)
        {
            application.MapPost("/imageMetadatas", Create);
            application.MapGet("/imageMetadatas", GetMany);
        }

        static async Task<IResult> Create([FromBody] ImageMetadata imageMetadata)
        {
            ImageMetadataRepository imageMetadataRepository = new ImageMetadataRepository();
            try
            {
                var imageUpdated = ImageMetadataEditor.UpdateExifMetadata(imageMetadata.Image, imageMetadata.Title,
                imageMetadata.Description, imageMetadata.CopyrightInformation, imageMetadata.Keywords);

                imageMetadata.Image = imageUpdated;

                await imageMetadataRepository.Create(imageMetadata);

                return Results.Ok();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

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
    }
}
