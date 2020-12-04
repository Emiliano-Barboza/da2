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
    public class SpotsRepository : BaseRepository<Spot>, ISpotsRepository
    {
        private const string CategorySpotKey = "CategorySpots";
        private readonly DbSet<Spot> spots;
        private readonly DbSet<Category> categories;

        public SpotsRepository(DbContext context) : base(context)
        {
            this.spots = context.Set<Spot>();
            this.categories = context.Set<Category>();
        }

        private bool UseCategorySpot(string order)
        {
            var useCategorySpot = false;

            if (!string.IsNullOrEmpty(order))
            {
               useCategorySpot = order == CategorySpotKey;   
            }

            return useCategorySpot;
        }

        private IQueryable<Spot> ExtendQueryWithFilterby(IQueryable<Spot> query, PagingModel pagingModel, bool useCategorySpots)
        {
            if (!string.IsNullOrEmpty(pagingModel.FilterBy))
            {
                if (useCategorySpots)
                {
                    query = query.Where(
                        x => x.CategorySpots.Any(y => pagingModel.FilterBy.Contains(y.CategoryId.ToString())));
                }
                else
                {
                    query = query.Where(
                        x => x.Name.ToUpper().Contains(pagingModel.FilterBy.ToUpper()));
                }
            }

            return query;
        }

        private async Task<PaginatedModel<Spot>> GetSpotsAsync(IQueryable<Spot> query, PagingModel pagingModel)
        {
            try
            {
                bool useCategorySpots = UseCategorySpot(pagingModel.Order);

                if (useCategorySpots)
                {
                    pagingModel.Order = PrimaryKey;
                }
                    
                query = ExtendQueryWithFilterby(query, pagingModel, useCategorySpots);

                var paginated = await GetPaginatedElementsAsync(query, pagingModel, PrimaryKey);
                return paginated;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get spots async", e);
            }
        }
        
        public async Task<Spot> GetSpotByIdAsync(int id)
        {
            try
            {
                var spot = await spots.Include("Region").FirstOrDefaultAsync(x => x.Id == id);
                return spot;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get spot by id async", e);
            }
        }

        public async Task<Spot> GetSpotByNameAsync(string name)
        {
            try
            {
                var spot = await spots.FirstOrDefaultAsync(x => x.Name == name);
                return spot;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get spot by name async", e);
            }
        }

        public async Task<Spot> AddSpotAsync(Spot spotModel)
        {
            try
            {
                var spot = spots.Add(spotModel);
                await context.SaveChangesAsync();
                return spot.Entity;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on add spot async", e);
            }
        }

        public async Task<PaginatedModel<Spot>> GetSpotsByRegionIdAsync(int regionId, PagingModel pagingModel)
        {
            try
            {
                IQueryable<Spot> query = spots.Where(x => x.RegionId == regionId).AsQueryable();
                var paginated = await GetSpotsAsync(query, pagingModel);
                return paginated;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get spots by region id async", e);
            }
        }

        public async Task<Category> GetSpotDefaultCategoryAsync()
        {
            try
            {
                var category = await categories.FirstOrDefaultAsync();
                return category;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get spot default category async", e);
            }
        }

        public async Task<PaginatedModel<Spot>> GetSpotsAsync(PagingModel pagingModel)
        {
            try
            {
                IQueryable<Spot> query = spots.AsQueryable();
                var paginated = await GetSpotsAsync(query, pagingModel);
                return paginated;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get spots async", e);
            }
        }

        public async Task<Spot> AddSpotImageAsync(int id, string url)
        {
            try
            {
                var spot = await spots.FirstOrDefaultAsync(x => x.Id == id);
                spot.Thumbnail = url;    
                await context.SaveChangesAsync();
                return spot;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on add spot image async", e);
            }
        }
    }
}