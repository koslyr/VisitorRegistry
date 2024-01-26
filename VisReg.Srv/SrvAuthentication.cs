using System;
using System.Text;
using System.Security.Cryptography;
using VisReg.Srv.Interfaces;

namespace VisReg.Srv
{
    public class SrvAuthentication : ISrvAuthentication
    {
        private string EncodePassword(string Password, string Salt)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] bytes = Encoding.UTF8.GetBytes(Password);
            byte[] src = Encoding.UTF8.GetBytes(Salt);
            //combine salt and hash
            byte[] dst = new byte[bytes.Length + src.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            byte[] inArray = sha1.ComputeHash(dst);

            return Convert.ToBase64String(inArray);
        }

        public bool VerifyHashedPassword(string HashedPasswordSalt, string ProvidedPassword)
        {
            string[] PassSalt = HashedPasswordSalt.Split(new[] { ':' });
            var salt = PassSalt[1];
            if (PassSalt.Length != 2) return false;

            return PassSalt[0] == EncodePassword(ProvidedPassword, salt) ? true : false;
        }
    }
}
