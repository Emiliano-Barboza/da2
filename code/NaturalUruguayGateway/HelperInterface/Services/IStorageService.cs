using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.HelperInterface.Services
{
    public interface IStorageService
    {
        Task<List<string>> AddLodgmentImageAsync(int id, FileModel fileModel);
        Task<Stream> GetLodgmentImageAsync(int id, string fileName);
        Task<string> AddSpotImageAsync(int id, FileModel fileModel);
        Task<Stream> GetSpotImageAsync(int id, string fileName);
    }
}