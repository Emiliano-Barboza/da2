using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayEngineInterface.Services
{
    public interface ISpotsService
    {
        Task<Spot> GetSpotByIdAsync(int id);
        Task<Spot> AddSpotAsync(Spot spotModel, Region regionModel);
        Task<Lodgment> AddLodgmentToSpotAsync(Lodgment lodgmentModel);
        Task<Lodgment> DeleteLodgmentInSpotAsync(int id, int lodgmentId);
        Task<PaginatedModel<Lodgment>> GetLodgmentsInSpotAsync(int id, LodgmentOptionsModel lodgmentOptionsModel, PagingModel pagingModel);
        Task<PaginatedModel<Spot>> GetSpotsByRegionIdAsync(int regionId, PagingModel pagingModel);
        Task<PaginatedModel<Spot>> GetSpotsAsync(PagingModel pagingModel);
        Task<string> UploadImageAsync(int id, FileModel fileModel);
        Task<Stream> GetImageAsync(int id, string fileName);
        Task<ReportModel<LodgmentsBookingsModel>> GetSpotReportAsync(int id, ReportOptionsModel reportOptionsModel);
    }
}