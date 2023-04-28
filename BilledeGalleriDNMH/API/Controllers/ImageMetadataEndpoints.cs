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
            application.MapPost("/imagemetadatas", Create);
        }

        async static Task<IResult> Create(IRepository<ImageMetadata> repository, IFormFile file, string title, string description, DateTime dateTime, string location, string camerainformation, string copyrightinformation, string[] keywords)
        {
            ImagaMetadataLogic imageMetadataLogic = new();

            var imageByte = await imageMetadataLogic.IFormFileTOByte(file);

            var imagewithmetadata = imageMetadataLogic.ExtractMetaDataFromByte(imageByte);
       
                //løsning lav nogle af variabler noget man SKAL udfylde.
                ImageMetadata imageMetadata = new ImageMetadata 
                { 
                    ImageByte = imageByte,
                    Title = title,
                    Description = description,
                    DateTime = dateTime,
                    Location = location,
                    CameraInformation = camerainformation,
                    CopyrightInformation = copyrightinformation,
                    Keywords = keywords,
                };


            var result = repository.Create(imageMetadata);

            return Results.Ok(result);
        }
    }
}
