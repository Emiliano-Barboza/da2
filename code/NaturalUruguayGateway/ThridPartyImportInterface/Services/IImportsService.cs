using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.ThridPartyImportInterface.Services
{
    public interface IImportsService
    {
        Task<List<ImportModel>> GetImportsAsync();
    }
}