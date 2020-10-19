using System;
using System.Security.Cryptography;

namespace ContentAggregator.Services.Helpers
{
    internal static class HashHelpers
    {
        internal static bool CheckPasswordWithHash(string password, string hash)
        {
            byte[] hashBytes = Convert.FromBase64String(hash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] bytes = pbkdf2.GetBytes(20);

            for (var i = 0; i < 20; i++)
                if (hashBytes[i + 16] != bytes[i])
                    return false;

            return true;
        }

        internal static string CreateHash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }
    }
}