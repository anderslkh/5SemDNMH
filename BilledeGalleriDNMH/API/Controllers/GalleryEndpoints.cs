using BLL;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDBRepository.Repositories;
using System.Xml.Linq;

namespace API.Controllers
{
    public static class GalleryEndpoints
    {
        public static void AddGalleryEndpoints(this WebApplication application)
        {
            application.MapPost("/Gallery", Create);
            application.MapGet("/Gallery", ReadOne);
            application.MapGet("/Galleries", ReadMany);
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

        static async Task<List<Gallery>> ReadMany()
        {
            GalleryRepository galleryRepository = new GalleryRepository();

            List<Gallery> galleryList = await galleryRepository.ReadMany();

            return galleryList;
        }
    }
}
