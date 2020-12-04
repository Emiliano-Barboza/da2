using System;
using System.Linq;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.EntityFrameworkCore;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories
{
    public class RegionsRepository : BaseRepository<Region>, IRegionsRepository
    {
        private readonly DbSet<Region> regions;

        public RegionsRepository(DbContext context) : base(context)
        {
            this.regions = context.Set<Region>();
        }
        
        public async Task<PaginatedModel<Region>> GetSpotsAsync(PagingModel pagingModel)
        {
            try
            {
                IQueryable<Region> query = regions.Select(x => x).AsQueryable();
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
                throw new WrappedDbException("Error on get spots async", e);
            }
        }

        public async Task<Region> GetRegionByIdAsync(int id)
        {
            try
            {
                var spot = await regions.FirstOrDefaultAsync(x => x.Id == id);
                return spot;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get spots ssync", e);
            }
        }
    }
}