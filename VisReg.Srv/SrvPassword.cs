using System;
using System.Security.Cryptography;
using System.Text;
using VisReg.Srv.Interfaces;

namespace VisReg.Srv
{
    public class SrvPassword : ISrvPassword
    {
        public string[] GetHashPasswordAndSalt(string UserPassword)
        {
            // Δημιουργία Salt & Hash με βάση το Password που δήλωσε ο χρήστης.
            string myPasswordAndSalt = HashPassword(UserPassword.Trim());
            string[] myPwdSlt = myPasswordAndSalt.Split(':');

            return myPwdSlt;
        }

        private string HashPassword(string Password)
        {
            var salt = GenerateSalt(16);

            return string.Format("{0}:{1}", EncodePassword(Password.Trim(), salt), salt);
        }

        // Δημιουργία Salt.
        private string GenerateSalt(short MaximumSaltLength)
        {
            // Empty salt array.
            var salt = new byte[MaximumSaltLength];

            using (var random = new RNGCryptoServiceProvider())
            {
                // Build the random bytes for the Salt.
                random.GetNonZeroBytes(salt);
            }

            // Return the string encoded salt.
            return Convert.ToBase64String(salt);
        }

        // Δημιουργία Κωδικοποιημένου Password.
        private string EncodePassword(string Password, string Salt)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] bytes = Encoding.UTF8.GetBytes(Password);
            byte[] src = Encoding.UTF8.GetBytes(Salt);

            // Combine salt and hash.
            byte[] dst = new byte[bytes.Length + src.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            byte[] inArray = sha1.ComputeHash(dst);

            return Convert.ToBase64String(inArray);
        }
    }
}
