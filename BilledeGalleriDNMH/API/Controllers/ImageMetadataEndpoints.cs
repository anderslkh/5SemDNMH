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
            ImagaMetadataLogic imageMetadataLogic = new();

            ImageMetadataRepository imageMetadataRepository = new ImageMetadataRepository();

            var imageWithMetadata = imageMetadataLogic.ExtractMetaDataFromByte(imageMetadata.Image);

            var imageUpdated = ImageMetadataEditor.UpdateExifMetadata(imageMetadata.Image, imageMetadata.Title, imageWithMetadata.Description, imageMetadata.CopyrightInformation, imageMetadata.Keywords);

            //løsning lav nogle af variabler noget man SKAL udfylde.
            ImageMetadata updatedImageMetadata = new ImageMetadata 
            { 
                Image = imageUpdated,
                Title = imageMetadata.Title,
                Description = imageMetadata.Description,
                DateTime = imageMetadata.DateTime,
                Location = imageMetadata.Location,
                CopyrightInformation = imageMetadata.CopyrightInformation,
                Keywords = imageMetadata.Keywords,
                ImageIdentifier = Guid.NewGuid().ToString(),
            };


            await imageMetadataRepository.Create(updatedImageMetadata);

            return Results.Ok();
        }

        static async Task<List<ImageMetadata>> GetMany([AsParameters] ImageMetadataQueryParameters imageMetadataQuery)
        {
            ImageMetadataRepository imageMetadataRepository = new();

            List<ImageMetadata> result = await imageMetadataRepository.ReadMany(imageMetadataQuery);

            return result;
        }
    }
}
