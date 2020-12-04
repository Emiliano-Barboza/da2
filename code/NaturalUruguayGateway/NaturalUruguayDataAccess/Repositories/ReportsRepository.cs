using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly DbSet<Booking> bookings;
        private const int MaxValidBookingStatus = 3;
        
        public ReportsRepository(DbContext context)
        {
            this.bookings = context.Set<Booking>();
        }

        public async Task<ReportModel<LodgmentsBookingsModel>> GetLodgmentsBookingsBySpotReportAsync(int id, ReportOptionsModel reportOptionsModel)
        {
            try
            {
                var report = new ReportModel<LodgmentsBookingsModel>();
                IQueryable<Booking> query = bookings.Where(x => x.BookingStatus.Id <= MaxValidBookingStatus && x.CheckIn >= reportOptionsModel.StartDate && x.CheckOut <= reportOptionsModel.EndDate).AsQueryable();
                query = query.Where(x => x.Lodgment.SpotId == id);
                report.Data = await query.GroupBy(p => p.Lodgment.Name)
                    .Select(g => new LodgmentsBookingsModel() { Name = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count).ToListAsync();
                return report;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get lodgments bookings by spot report async", e);
            }
        }
    }
}