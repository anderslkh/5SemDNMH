using BLL;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDBRepository.Repositories;

namespace API.Controllers
{
    public static class GalleryEndpoints
    {
        public static void AddGalleryEndpoints(this WebApplication application)
        {
            application.MapGet("/gallery", ReadOne);
            application.MapPost("/user", Create);
        }

        static async Task<IResult> Create([FromBody] Gallery gallery)
        {
            GalleryRepository galleryRepository = new GalleryRepository();
            try
            {
                await galleryRepository.Create(gallery);

                return Results.Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        static async Task<Gallery> ReadOne(string name)
        {
            GalleryRepository galleryRepository = new GalleryRepository();

            Gallery gallery = await galleryRepository.ReadOne(name);

            return gallery;
        }
    }
}
