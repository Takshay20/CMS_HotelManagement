using System;
using System.Security.Cryptography;

namespace CMS_HotelBooking.Helpers
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        // Create password hash
        public static string Hash(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256);
            var salt = algorithm.Salt;
            var key = algorithm.GetBytes(KeySize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        // Check password against saved hash
        public static bool Verify(string password, string hashedValue)
        {
            var parts = hashedValue.Split('.', 3);
            if (parts.Length != 3) return false;

            var iterations = int.Parse(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var keyToCheck = algorithm.GetBytes(key.Length);

            return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
        }
    }
}
