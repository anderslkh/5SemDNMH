using BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using MongoDBRepository;
using MongoDBRepository.Repositories;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageMetadataController
    {
        //public static void AddImageMetaDataEndpoints(this WebApplication application)
        //{
        //    application.MapPost("/imageMetadatas", Create);
        //}


        [HttpPost]
        [Route("Create")]
        public async Task<IResult> Create(IRepository<ImageMetadata> repository, [FromForm] Image image)
        {
            ImagaMetadataLogic imageMetadataLogic = new();

            var imageFile = image.ImageFile;

            var imageByte = await imageMetadataLogic.IFormFileToByte(imageFile);

            var imageWithMetadata = imageMetadataLogic.ExtractMetaDataFromByte(imageByte);
       
                //løsning lav nogle af variabler noget man SKAL udfylde.
                ImageMetadata imageMetadata = new ImageMetadata 
                { 
                    Image = imageByte,
                    Title = image.Title,
                    Description = imageWithMetadata.Description,
                    DateTime = imageWithMetadata.DateTime,
                    Location = imageWithMetadata.Location,
                    CameraInformation = imageWithMetadata.CameraInformation,
                    CopyrightInformation = image.CopyrightInformation,
                    Keywords = image.Keywords,
                };


            var result = repository.Create(imageMetadata);

            return Results.Ok(result);
        }
    }
}
