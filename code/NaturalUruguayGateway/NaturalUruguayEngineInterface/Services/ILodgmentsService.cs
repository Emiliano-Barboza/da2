using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayEngineInterface.Services
{
    public interface ILodgmentsService
    {
        Task<Lodgment> AddLodgmentAsync(Lodgment lodgmentModel, Spot spotModel);
        Task<Lodgment> DeleteLodgmentAsync(int id);
        Task<Lodgment> SetLodgmentActiveStatusAsync(int id, bool isActive);
        Task<PaginatedModel<Lodgment>> GetLodgmentsAsync(PagingModel pagingModel, 
            LodgmentOptionsModel? lodgmentOptionsModel = null, int? spotId = null, bool? includeDeactivated = false);
        Task<Lodgment> GetLodgmentByIdAsync(int id, LodgmentOptionsModel? lodgmentOptionsModel = null);
        Task<List<string>> UploadImageAsync(int id, FileModel fileModel);
        Task<Stream> GetImageAsync(int id, string fileName);
        Task<PaginatedModel<Review>> GetLodgmentReviewsAsync(int id, PagingModel pagingModel);
        Task<Review> AddLodgmentReviewAsync(int id, Review reviewModel);
        Task<Booking> AddLodgmentBookingAsync(int id, BookingConfirmationModel bookingConfirmationModel);
    }
}