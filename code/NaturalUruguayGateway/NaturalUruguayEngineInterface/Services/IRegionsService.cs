using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayEngineInterface.Services
{
    public interface IRegionsService
    {
        Task<PaginatedModel<Region>> GetRegionsAsync(PagingModel pagingModel);
        Task<Region> GetRegionByIdAsync(int id);
        Task<Spot> AddSpotToRegionAsync(Spot spotModel);
        Task<PaginatedModel<Spot>> GetSpotsByRegionAsync(int id, PagingModel pagingModel);
    }
}