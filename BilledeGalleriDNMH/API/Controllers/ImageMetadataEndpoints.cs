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
       
                //løsning lav nogle af variabler noget man SKAL udfylde.
                ImageMetadata imageMetadata = new ImageMetadata 
                { 
                    Image = imageFile.ImageByte,
                    Title = imageFile.Title,
                    Description = imageWithMetadata.Description,
                    DateTime = (DateTime)imageWithMetadata.DateTime,
                    Location = imageWithMetadata.Location,
                    CameraInformation = imageWithMetadata.CameraInformation,
                    CopyrightInformation = imageFile.CopyrightInformation,
                    Keywords = imageFile.Keywords,
                };

            await imageMetadataRepository.Create(imageMetadata);

            return Results.Ok(StatusCodes.Status200OK);
        }

        static async Task<IResult> GetMany([AsParameters] ImageMetadataQueryParameters imageMetadataQuery)
        {
            ImageMetadataRepository imageMetadataRepository = new ImageMetadataRepository();

            List<ImageMetadata> result = await imageMetadataRepository.ReadMany(imageMetadataQuery);

            return Results.Ok(result);
        }
    }
}
