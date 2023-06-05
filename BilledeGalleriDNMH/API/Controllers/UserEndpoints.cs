using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDBRepository.Repositories;

namespace API.Controllers
{
    public static class UserEndpoints
    {
        public static void AddUserEndpoints(this WebApplication application)
        {
            application.MapGet("/user", ReadOne);
            application.MapPost("/user", Create);
        }

        static async Task<IResult> Create([FromBody] User receivedUser)
        {
            PasswordLogic passwordLogic = new PasswordLogic();
            UserRepository userRepository = new();
    
            User hashedUser = passwordLogic.HashPassword(receivedUser);

            await userRepository.Create(hashedUser);

            return Results.Ok();
        }

        static async Task<User> ReadOne(string email)
        {
            UserRepository userRepository = new();

            User user = await userRepository.ReadOne(email);

            return user;
        }
    }
}
