using System.Threading.Tasks;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories
{
    public interface IReportsRepository
    {
        Task<ReportModel<LodgmentsBookingsModel>> GetLodgmentsBookingsBySpotReportAsync(int id, ReportOptionsModel reportOptionsModel);
    }
}