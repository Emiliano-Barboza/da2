using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.EntityFrameworkCore;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories
{
    public class LodgmentsRepository : BaseRepository<Lodgment>, ILodgmentsRepository
    {
        private readonly DbSet<Lodgment> lodgments;

        public LodgmentsRepository(DbContext context) : base(context)
        {
            this.lodgments = context.Set<Lodgment>();
        }
        public async Task<Lodgment> AddLodgmentAsync(Lodgment lodgmentModel)
        {
            try
            {
                lodgmentModel.IsActive = true;
                var lodgment = lodgments.Add(lodgmentModel);
                await context.SaveChangesAsync();
                return lodgment.Entity;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on add lodgment async", e);
            }
        }

        public async Task<Lodgment> GetLodgmentByNameAsync(string name)
        {
            try
            {
                var lodgment = await lodgments.FirstOrDefaultAsync(x => x.Name == name && !x.IsDeleted && x.IsActive);
                return lodgment;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get lodgment by name async", e);
            }
        }

        public async Task<Lodgment> GetLodgmentByIdAsync(int id)
        {
            try
            {
                var lodgment = await lodgments
                    .Include(x => x.Spot)
                    .ThenInclude(x => x.Region)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
                return lodgment;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get lodgment by id async", e);
            }
        }

        public async Task<Lodgment> DeleteLodgmentAsync(int id)
        {
            try
            {
                var lodgment = await lodgments.FirstOrDefaultAsync(x => x.Id == id);
                lodgment.IsDeleted = true;
                await context.SaveChangesAsync();
                return lodgment;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on delete lodgment async", e);
            }
        }

        public async Task<Lodgment> SetLodgmentActiveStatusAsync(int id, bool isActive)
        {
            try
            {
                var lodgment = await lodgments.FirstOrDefaultAsync(x => x.Id == id);
                lodgment.IsActive = isActive;
                await context.SaveChangesAsync();
                return lodgment;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on set lodgment active status async", e);
            }
        }

        public async Task<Lodgment> AddLodgmentImageAsync(int id, IEnumerable<string> urls)
        {
            try
            {
                var lodgment = await lodgments.FirstOrDefaultAsync(x => x.Id == id);
                lodgment.Images = lodgment.Images.Concat(urls).ToList();    
                await context.SaveChangesAsync();
                return lodgment;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on add lodgment image async", e);
            }
        }

        public async Task<PaginatedModel<Lodgment>> GetLodgmentsAsync(PagingModel pagingModel, int? spotId = null, bool? includeDeactivated = false)
        {
            try
            {
                await Task.Yield();
                IQueryable<Lodgment> query = null;

                if (includeDeactivated.HasValue && includeDeactivated.Value)
                {
                    query = lodgments.Where(x => !x.IsDeleted).AsQueryable();
                }
                else
                {
                    query = lodgments.Where(x => !x.IsDeleted && x.IsActive).AsQueryable();
                }

                if (spotId.HasValue)
                {
                    query = query.Where(x => x.SpotId == spotId.Value);
                }
            
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
                throw new WrappedDbException("Error on get lodgments async", e);
            }
        }
    }
}