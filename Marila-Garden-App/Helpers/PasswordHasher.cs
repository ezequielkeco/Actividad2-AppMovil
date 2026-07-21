using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Marila_Garden_App.Helpers
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            byte[] passwordBytes =
                Encoding.UTF8.GetBytes(password);

            byte[] hashBytes =
                SHA256.HashData(passwordBytes);

            return Convert.ToHexString(hashBytes);
        }

        public static bool Verify(
            string password,
            string passwordHash)
        {
            string calculatedHash = Hash(password);

            return string.Equals(
                calculatedHash,
                passwordHash,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
