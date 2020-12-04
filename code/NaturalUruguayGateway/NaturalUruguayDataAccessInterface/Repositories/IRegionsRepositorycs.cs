using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories
{
    public interface IRegionsRepository
    {
        Task<PaginatedModel<Region>> GetSpotsAsync(PagingModel pagingModel);
        Task<Region> GetRegionByIdAsync(int id);
    }
}