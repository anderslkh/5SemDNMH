﻿using System.Security.Cryptography;
using System.Text;

namespace BLL
{
    public static class PasswordHelper
    {
        //Metode til at oprette en Salt
        public static string GenerateSalt()
        {
            const int length = 32;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[length];

            rng.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }

        //Hasher et password med salt. Bruger SHA512
        public static string ComputeHash(string password, string salt)
        {
            SHA512Managed sHa512ManagedString = new SHA512Managed();
            byte[] bytes = sHa512ManagedString.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X2"));
            }

            return builder.ToString();
        }

        //Checker om et password stemmer overens med det hashede password
        public static bool ComparePass(string password, string passwordHash, string salt)
        {
            string newHashedPin = ComputeHash(password, salt);
            return newHashedPin.Equals(passwordHash);
        }
    }
}