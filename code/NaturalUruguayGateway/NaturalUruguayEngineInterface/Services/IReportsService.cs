using System.Threading.Tasks;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayEngineInterface.Services
{
    public interface IReportsService
    {
        Task<ReportModel<LodgmentsBookingsModel>> GetReportBySpotAsync(int id, ReportOptionsModel reportOptionsModel);
    }
}