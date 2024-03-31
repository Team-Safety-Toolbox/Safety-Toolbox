using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Safety_Toolbox.Types
{
    internal class Password
    {
        private const int keySize = 64;
        private const int iterations = 350000;
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private string PlainTextPassword {  get; set; }
        public byte[] Salt { get; set; }

        public Password() { }
        public Password(string password)
        {
            PlainTextPassword = password;
        }

        public Password(string password, byte[] existingSalt)
        {
            PlainTextPassword = password;
            Salt = existingSalt;
        }

        public byte[] Hash()
        {
            if (Salt is null)
            {
                Salt = RandomNumberGenerator.GetBytes(keySize);
            }

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(PlainTextPassword),
                Salt,
                iterations,
                hashAlgorithm,
                keySize);

            return hash;
        }



    }
}
