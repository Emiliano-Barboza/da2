using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories
{
    public class ReviewsRepository : BaseRepository<Review>, IReviewsRepository
    {
        private readonly DbSet<Review> reviews;
        
        public ReviewsRepository(DbContext context) : base(context)
        {
            this.reviews = context.Set<Review>();
        }
        
        public async Task<PaginatedModel<Review>> GetLodgmentReviewsAsync(int id, PagingModel pagingModel)
        {
            try
            {
                await Task.Yield();
                IQueryable<Review> query = reviews.Where(x => x.LodgmentId == id).AsQueryable();

                if (!string.IsNullOrEmpty(pagingModel.FilterBy))
                {
                    query = query.Where(
                        x => x.Name.ToUpper().Contains(pagingModel.FilterBy.ToUpper()));
                }
            
                var paginated = await GetPaginatedElementsAsync(query, pagingModel, PrimaryKey);
                return paginated;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get reviews async", e);
            }
        }

        public async Task<Review> AddLodgmentReviewAsync(Review reviewModel)
        {
            try
            {
                var review = reviews.Add(reviewModel);
                await context.SaveChangesAsync();
                return review.Entity;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on add lodgment review async", e);
            }
        }

        public async Task<Review> GetReviewAsync(string confirmationCode)
        {
            try
            {
                var review = await reviews.FirstOrDefaultAsync(x => x.ConfirmationCode == confirmationCode);
                return review;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get review async", e);
            }
        }
    }
}