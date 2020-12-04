using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories
{
    public interface IReviewsRepository
    {
        Task<PaginatedModel<Review>> GetLodgmentReviewsAsync(int id, PagingModel pagingModel);
        Task<Review> AddLodgmentReviewAsync(Review reviewModel);
        Task<Review> GetReviewAsync(string confirmationCode);
    }
}