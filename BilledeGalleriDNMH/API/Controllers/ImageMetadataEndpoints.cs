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

        static async Task<IResult> Create([FromBody] ImageFile imageFile)
        {
            ImagaMetadataLogic imageMetadataLogic = new();

            ImageMetadataRepository imageMetadataRepository = new ImageMetadataRepository();

            var imageWithMetadata = imageMetadataLogic.ExtractMetaDataFromByte(imageFile.ImageByte);

            var imageUpdated = ImageMetadataEditor.UpdateExifMetadata(imageFile.ImageByte, imageFile.Title, imageWithMetadata.Description, imageFile.CopyrightInformation, imageFile.Keywords);

            //løsning lav nogle af variabler noget man SKAL udfylde.
            ImageMetadata imageMetadata = new ImageMetadata 
            { 
                Image = imageUpdated,
                Title = imageFile.Title,
                Description = imageFile.Description,
                DateTime = (DateTime)imageWithMetadata.DateTime,
                Location = imageFile.Location,
                CopyrightInformation = imageFile.CopyrightInformation,
                Keywords = imageFile.Keywords,
                ImageIdentifier = Guid.NewGuid().ToString(),
            };


            await imageMetadataRepository.Create(imageMetadata);

            return Results.Ok(StatusCodes.Status200OK);
        }

        static async Task<List<ImageMetadata>> GetMany([AsParameters] ImageMetadataQueryParameters imageMetadataQuery)
        {
            ImageMetadataRepository imageMetadataRepository = new();

            List<ImageMetadata> result = await imageMetadataRepository.ReadMany(imageMetadataQuery);

            return result;
        }
    }
}
