using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.HelperInterface.Services;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Services
{
    public class SpotsService : ISpotsService
    {
        private readonly ISpotsRepository repository;
        private readonly ILodgmentsService lodgmentsService;
        private readonly IStorageService storageService;
        private readonly IReportsService reportsService;
        
        
        public SpotsService(ISpotsRepository repository, ILodgmentsService lodgmentsService,
            IStorageService storageService, IReportsService reportsService)
        {
            this.repository = repository;
            this.lodgmentsService = lodgmentsService;
            this.storageService = storageService;
            this.reportsService = reportsService;
        }

        private async Task CheckSpotExists(int id)
        {
            var spot = await repository.GetSpotByIdAsync(id);
            if (spot == null)
            {
                throw new KeyNotFoundException("The spot doesn't exists.");
            }
        }
        
        public async Task<Spot> GetSpotByIdAsync(int id)
        {
            var spot = await repository.GetSpotByIdAsync(id);
            return spot;
        }

        public async Task<Spot> AddSpotAsync(Spot spotModel, Region regionModel)
        {
            if (spotModel == null)
            {
                throw new ArgumentException("Must provide a spot.");
            }
            if (regionModel == null)
            {
                throw new ArgumentException("Must provide a region.");
            }
            if (spotModel.RegionId != regionModel.Id)
            {
                throw new ArgumentException("The region for the spot is different from the current region.");
            }
            
            if (spotModel.CategorySpots == null)
            {
                var category = await repository.GetSpotDefaultCategoryAsync();
                var categorySpot = new CategorySpot();
                categorySpot.CategoryId = category.Id;
                spotModel.CategorySpots = new List<CategorySpot>();
                spotModel.CategorySpots.Add(categorySpot);
            }
            
            var spot = await repository.GetSpotByNameAsync(spotModel.Name);
            if (spot != null)
            {
                throw new DuplicateNameException("The spot already exists.");
            }
            
            spot = await repository.AddSpotAsync(spotModel);
            return spot;
        }

        public async Task<Lodgment> AddLodgmentToSpotAsync(Lodgment lodgmentModel)
        {
            var spot = await repository.GetSpotByIdAsync(lodgmentModel.SpotId);
            if (spot == null)
            {
                throw new KeyNotFoundException("The spot doesn't exists.");
            }
            
            var lodgment = await lodgmentsService.AddLodgmentAsync(lodgmentModel, spot);
            return lodgment;
        }

        public async Task<Lodgment> DeleteLodgmentInSpotAsync(int id, int lodgmentId)
        {
            var spot = await repository.GetSpotByIdAsync(id);
            if (spot == null)
            {
                throw new KeyNotFoundException("The spot doesn't exists.");
            }

            var lodgment = await lodgmentsService.DeleteLodgmentAsync(lodgmentId);
            return lodgment;
        }

        public async Task<PaginatedModel<Lodgment>> GetLodgmentsInSpotAsync(int id, LodgmentOptionsModel lodgmentOptionsModel, 
            PagingModel pagingModel)
        {
            var paginatedLodgments = await lodgmentsService.GetLodgmentsAsync(pagingModel, 
                lodgmentOptionsModel, id);
            return paginatedLodgments;
        }

        public async Task<PaginatedModel<Spot>> GetSpotsByRegionIdAsync(int regionId, PagingModel pagingModel)
        {
            var paginatedSpots = await repository.GetSpotsByRegionIdAsync(regionId, pagingModel);
            return paginatedSpots;
        }

        public async Task<PaginatedModel<Spot>> GetSpotsAsync(PagingModel pagingModel)
        {
            var paginatedSpots = await repository.GetSpotsAsync(pagingModel);
            return paginatedSpots;
        }
        
        public async Task<string> UploadImageAsync(int id, FileModel fileModel)
        {
            string url = null;
            var spot = await repository.GetSpotByIdAsync(id);

            if (spot == null)
            {
                throw new KeyNotFoundException();
            }

            url = await storageService.AddSpotImageAsync(id, fileModel);
            
            await repository.AddSpotImageAsync(id, url);

            return url;
        }

        public async Task<Stream> GetImageAsync(int id, string fileName)
        {
            Stream stream = null;
            var spot = await repository.GetSpotByIdAsync(id);

            if (spot == null)
            {
                throw new KeyNotFoundException();
            }
            
            stream = await storageService.GetSpotImageAsync(id, fileName);
            return stream;
        }

        public async Task<ReportModel<LodgmentsBookingsModel>> GetSpotReportAsync(int id, ReportOptionsModel reportOptionsModel)
        {
            await CheckSpotExists(id);
            
            var report = await reportsService.GetReportBySpotAsync(id, reportOptionsModel);
            return report;
        }
    }
}