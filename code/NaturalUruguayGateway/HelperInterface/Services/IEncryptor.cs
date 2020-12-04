using System.Threading.Tasks;

namespace NaturalUruguayGateway.HelperInterface.Services
{
    public interface IEncryptor
    {
        Task<string> EncryptAsync(string key);
        Task<bool> ValidateAsync(string key, string hash);
    }
}