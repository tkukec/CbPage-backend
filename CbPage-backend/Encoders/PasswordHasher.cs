using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CbPage_backend.Encoders {
    public static class PassowrdHasher
    {
        const int hashCount = 10;
        public static byte[] Hash(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] bytes = (byte[])passwordBytes.Concat(salt);
            using (SHA512 sha512 = SHA512.Create())
                for (int i = 0; i < hashCount; i++)
                    bytes = sha512.ComputeHash(bytes);

            return bytes;
        }
    }
}
