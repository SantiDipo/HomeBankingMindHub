using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace HomeBankingMindHub.Utils
{
    public class Hasher
    {
        public static string HashPassword(string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes("santipassword");       

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
