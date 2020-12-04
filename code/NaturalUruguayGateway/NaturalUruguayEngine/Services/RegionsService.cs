using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Services
{
    public class RegionsService : IRegionsService
    {
        private readonly IRegionsRepository repository;
        private readonly ISpotsService spotsService;
        
        public RegionsService(IRegionsRepository repository, ISpotsService spotsService)
        {
            this.repository = repository;
            this.spotsService = spotsService;
        }
        
        public async Task<PaginatedModel<Region>> GetRegionsAsync(PagingModel pagingModel)
        {
            var regions = await repository.GetSpotsAsync(pagingModel);
            return regions;
        }
        
        public async Task<Region> GetRegionByIdAsync(int id)
        {
            var spot = await repository.GetRegionByIdAsync(id);
            return spot;
        }

        public async Task<Spot> AddSpotToRegionAsync(Spot spotModel)
        {
            var region = await repository.GetRegionByIdAsync(spotModel.RegionId);
            if (region == null)
            {
                throw new KeyNotFoundException("La región no existe.");
            }
            
            var spot = await spotsService.AddSpotAsync(spotModel, region);
            return spot;
        }

        public async Task<PaginatedModel<Spot>> GetSpotsByRegionAsync(int id, PagingModel pagingModel)
        {
            var region = await repository.GetRegionByIdAsync(id);
            if (region == null)
            {
                throw new KeyNotFoundException("Region not exists.");
            }
            
            var spot = await spotsService.GetSpotsByRegionIdAsync(id, pagingModel);
            return spot;
        }
    }
}