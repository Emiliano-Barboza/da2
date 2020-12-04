using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NaturalUruguayGateway.HelperInterface.Services;

namespace NaturalUruguayGateway.Helper.Services
{
    public class MD5Encryptor : IEncryptor
    {
        private async Task<string> BytesToStringAsync(byte[] input)
        {
            await Task.Yield();
            var builder = new StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                builder.Append(input[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public async Task<string> EncryptAsync(string key)
        {
            var bytes = Encoding.ASCII.GetBytes(key);
            byte[] md5Data = null;
            using (var md5Hash = MD5.Create())
            {
                md5Data = md5Hash.ComputeHash(bytes);
            }

            var hash = await BytesToStringAsync(md5Data);
            return hash;
        }

        public async Task<bool> ValidateAsync(string key, string hash)
        {
            var keyHash = await EncryptAsync(key);
            bool isValid = keyHash.Equals(hash, StringComparison.InvariantCultureIgnoreCase);
            return isValid;
        }
    }
}