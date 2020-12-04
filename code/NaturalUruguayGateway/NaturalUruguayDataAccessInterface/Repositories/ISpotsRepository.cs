using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories
{
    public interface ISpotsRepository
    {
        Task<Spot> GetSpotByIdAsync(int id);
        Task<Spot> GetSpotByNameAsync(string name);
        Task<Spot> AddSpotAsync(Spot spot);
        Task<PaginatedModel<Spot>> GetSpotsByRegionIdAsync(int regionId, PagingModel pagingModel);
        Task<Category> GetSpotDefaultCategoryAsync();
        Task<PaginatedModel<Spot>> GetSpotsAsync(PagingModel pagingModel);
        Task<Spot> AddSpotImageAsync(int id, string url);
    }
}