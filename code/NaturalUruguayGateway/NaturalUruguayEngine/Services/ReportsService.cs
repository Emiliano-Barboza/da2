using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository repository;
        private readonly string reportLodgmentsBookingName;
        
        
        public ReportsService(IReportsRepository repository)
        {
            this.repository = repository;
            this.reportLodgmentsBookingName = "lodgments-bookings";
        }
        
        private async Task<ReportModel<LodgmentsBookingsModel>> GetLodgmentsBookingsBySpotReportAsync(int id, ReportOptionsModel reportOptionsModel)
        {
            var report = await repository.GetLodgmentsBookingsBySpotReportAsync(id, reportOptionsModel);
            return report;
        }
        
        public async Task<ReportModel<LodgmentsBookingsModel>> GetReportBySpotAsync(int id, ReportOptionsModel reportOptionsModel)
        {
            ReportModel<LodgmentsBookingsModel> report = null;
            if (reportOptionsModel.Name == this.reportLodgmentsBookingName)
            {
                report = await GetLodgmentsBookingsBySpotReportAsync(id, reportOptionsModel);
            }
            else
            {
                throw new KeyNotFoundException("Reporte inexistente");
            }

            return report;
        }
    }
}