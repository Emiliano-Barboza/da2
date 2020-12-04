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
    public class LodgmentsService : ILodgmentsService
    {
        private readonly ILodgmentsRepository repository;
        private readonly ILodgmentCalculator lodgmentCalculator;
        private readonly IStorageService storageService;
        private readonly IReviewsService reviewsService;
        private readonly IBookingsService bookingsService;

        public LodgmentsService(ILodgmentsRepository repository, ILodgmentCalculator lodgmentCalculator,
            IStorageService storageService, IReviewsService reviewsService, IBookingsService bookingsService)
        {
            this.repository = repository;
            this.lodgmentCalculator = lodgmentCalculator;
            this.storageService = storageService;
            this.reviewsService = reviewsService;
            this.bookingsService = bookingsService;
        }

        private async Task<bool> CheckLodgmentOptionsValid(LodgmentOptionsModel lodgmentOptionsModel)
        {
            await Task.Yield();
            bool isValid = lodgmentOptionsModel != null;
            
            if (isValid)
            {
                isValid = lodgmentOptionsModel.CheckIn > 0 ||
                          lodgmentOptionsModel.CheckOut > 0;
            }

            return isValid;
        }
        
        public async Task<Lodgment> AddLodgmentAsync(Lodgment lodgmentModel, Spot spotModel)
        {
            if (lodgmentModel == null)
            {
                throw new ArgumentException("Debe proveer un alojamiento.");
            }

            if (spotModel == null)
            {
                throw new ArgumentException("Debe proveer una punto turístico.");
            }

            if (lodgmentModel.SpotId != spotModel.Id)
            {
                throw new ArgumentException(
                    "El punto turístico para el alojamiento es distinto del punto turístico actual.");
            }

            var lodgment = await repository.GetLodgmentByNameAsync(lodgmentModel.Name);
            if (lodgment != null)
            {
                throw new DuplicateNameException("El hospedaje ya existe.");
            }

            lodgment = await repository.AddLodgmentAsync(lodgmentModel);
            return lodgment;
        }

        public async Task<Lodgment> DeleteLodgmentAsync(int id)
        {
            var lodgment = await repository.GetLodgmentByIdAsync(id);
            if (lodgment == null)
            {
                throw new KeyNotFoundException("El hospedaje no existe.");
            }

            lodgment = await repository.DeleteLodgmentAsync(id);
            return lodgment;
        }

        public async Task<Lodgment> SetLodgmentActiveStatusAsync(int id, bool isActive)
        {
            var lodgment = await repository.GetLodgmentByIdAsync(id);
            if (lodgment == null)
            {
                throw new KeyNotFoundException("El hospedaje no existe.");
            }

            lodgment = await repository.SetLodgmentActiveStatusAsync(id, isActive);
            return lodgment;
        }
        
        public async Task<PaginatedModel<Lodgment>> GetLodgmentsAsync(PagingModel pagingModel,
            LodgmentOptionsModel? lodgmentOptionsModel = null, int? spotId = null, bool? includeDeactivated = false)
        {
            var paginatedLodgments = await repository.GetLodgmentsAsync(pagingModel, spotId, includeDeactivated);

            foreach (var lodgment in paginatedLodgments.Data)
            {
                lodgment.TotalPrice = await lodgmentCalculator.CalculateTotalStayAsync(lodgmentOptionsModel, lodgment);
            }

            return paginatedLodgments;
        }

        public async Task<Lodgment> GetLodgmentByIdAsync(int id, LodgmentOptionsModel lodgmentOptionsModel = null)
        {
            var lodgment = await repository.GetLodgmentByIdAsync(id);
            if (lodgment != null)
            {
                var isValid = await CheckLodgmentOptionsValid(lodgmentOptionsModel);
                if (isValid)
                {
                    lodgment.TotalPrice = await lodgmentCalculator.CalculateTotalStayAsync(lodgmentOptionsModel, lodgment);    
                }
            }
            return lodgment;
        }

        public async Task<List<string>> UploadImageAsync(int id, FileModel fileModel)
        {
            List<string> urls = null;
            var lodgment = await repository.GetLodgmentByIdAsync(id);

            if (lodgment == null)
            {
                throw new KeyNotFoundException();
            }

            urls = await storageService.AddLodgmentImageAsync(id, fileModel);
            
            await repository.AddLodgmentImageAsync(id, urls);

            return urls;
        }

        public async Task<Stream> GetImageAsync(int id, string fileName)
        {
            Stream stream = null;
            var lodgment = await repository.GetLodgmentByIdAsync(id);

            if (lodgment == null)
            {
                throw new KeyNotFoundException();
            }
            
            stream = await storageService.GetLodgmentImageAsync(id, fileName);
            return stream;
        }

        public async Task<PaginatedModel<Review>> GetLodgmentReviewsAsync(int id, PagingModel pagingModel)
        {
            var lodgment = await repository.GetLodgmentByIdAsync(id);
            if (lodgment == null)
            {
                throw new KeyNotFoundException("El hospedaje no existe.");
            }
            
            var paginatedReviews = await reviewsService.GetLodgmentReviewsAsync(id, pagingModel);
            return paginatedReviews;
        }

        public async Task<Review> AddLodgmentReviewAsync(int id, Review reviewModel)
        {
            var lodgment = await repository.GetLodgmentByIdAsync(id);
            if (lodgment == null)
            {
                throw new KeyNotFoundException("El hospedaje no existe.");
            }
            var review = await reviewsService.AddLodgmentReviewAsync(id, reviewModel);
            return review;
        }

        public async Task<Booking> AddLodgmentBookingAsync(int id, BookingConfirmationModel bookingConfirmationModel)
        {
            var lodgment = await repository.GetLodgmentByIdAsync(id);
            if (lodgment == null)
            {
                throw new ArgumentException("The lodgment is out of capacity");
            }
            
            var booking = await bookingsService.AddBookingAtLodgmentAsync(lodgment, bookingConfirmationModel);
            return booking;
        }
    }
}