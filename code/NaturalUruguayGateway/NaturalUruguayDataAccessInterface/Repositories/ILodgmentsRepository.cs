using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories
{
    public interface ILodgmentsRepository
    {
        Task<Lodgment> AddLodgmentAsync(Lodgment lodgmentModel);
        Task<Lodgment> GetLodgmentByNameAsync(string name);
        Task<Lodgment> GetLodgmentByIdAsync(int id);
        Task<Lodgment> DeleteLodgmentAsync(int id);
        Task<Lodgment> SetLodgmentActiveStatusAsync(int id, bool isActive);
        Task<Lodgment> AddLodgmentImageAsync(int id, IEnumerable<string> urls);
        Task<PaginatedModel<Lodgment>> GetLodgmentsAsync(PagingModel pagingModel, int? spotId = null, bool? includeDeactivated = false);
    }
}