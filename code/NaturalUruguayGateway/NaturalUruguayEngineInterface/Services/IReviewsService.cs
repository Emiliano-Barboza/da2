using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayEngineInterface.Services
{
    public interface IReviewsService
    {
        Task<PaginatedModel<Review>> GetLodgmentReviewsAsync(int id, PagingModel pagingModel);
        Task<Review> AddLodgmentReviewAsync(int id, Review reviewModel);
    }
}