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
        private readonly ImageMetadataRepository _imageMetadataRepository;

        public ImageMetadataController()
        {
            _imageMetadataRepository = new ImageMetadataRepository();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IResult> Create([FromForm] ImageFile image)
        {
            ImagaMetadataLogic imageMetadataLogic = new();

            var imageFile = image.ImageFormFile;

            var imageByte = await imageMetadataLogic.IFormFileToByte(imageFile);

            var imageWithMetadata = imageMetadataLogic.ExtractMetaDataFromByte(imageByte);
       
                //løsning lav nogle af variabler noget man SKAL udfylde.
                ImageMetadata imageMetadata = new ImageMetadata 
                { 
                    Image = imageByte,
                    Title = image.Title,
                    Description = imageWithMetadata.Description,
                    DateTime = (DateTime)imageWithMetadata.DateTime,
                    Location = imageWithMetadata.Location,
                    CameraInformation = imageWithMetadata.CameraInformation,
                    CopyrightInformation = image.CopyrightInformation,
                    Keywords = image.Keywords,
                };


            await _imageMetadataRepository.Create(imageMetadata);

            return Results.Ok(StatusCodes.Status200OK);
        }
    }
}
