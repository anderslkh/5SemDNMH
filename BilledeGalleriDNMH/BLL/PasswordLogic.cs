using Models;
using MongoDBRepository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PasswordLogic
    {
        public User HashPassword(User user)
        {
            string salt = PasswordHelper.GenerateSalt();

            string hashedPassword = PasswordHelper.ComputeHash(user.Password, salt);

            User userWithHashedPassword = new User
            {
                Email = user.Email,
                Password = hashedPassword,
                Salt = salt
            };

            return userWithHashedPassword;
        }

        public async Task<bool> CheckIfPasswordsMatch(string email, string password)
        {
            UserRepository userRepository = new UserRepository();

            User user = await userRepository.ReadOne(email);

            bool sucess = PasswordHelper.ComparePass(password, user.Password, user.Salt);

            return sucess;
        }
    }
}