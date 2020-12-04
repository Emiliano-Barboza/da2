using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly IReviewsRepository repository;
        private readonly IBookingsService bookingService;
        
        public ReviewsService(IReviewsRepository repository, IBookingsService bookingService)
        {
            this.repository = repository;
            this.bookingService = bookingService;
        }
        
        public async Task<PaginatedModel<Review>> GetLodgmentReviewsAsync(int id, PagingModel pagingModel)
        {
            var paginatedReviews = await repository.GetLodgmentReviewsAsync(id, pagingModel);

            return paginatedReviews;
        }

        public async Task<Review> AddLodgmentReviewAsync(int id, Review reviewModel)
        {
            var booking = await bookingService.GetBookingStatusAsync(reviewModel.ConfirmationCode);

            if (booking == null)
            {
                throw new KeyNotFoundException("El código de reserva no existe.");
            }
            
            var review = await repository.GetReviewAsync(reviewModel.ConfirmationCode);
            
            if (review != null)
            {
                throw new ArgumentException("El código de reserva ya fue usado.");
            }
            reviewModel.LodgmentId = id;
            reviewModel.Name = booking.Name;
            
            review = await repository.AddLodgmentReviewAsync(reviewModel);
            return review;
        }
    }
}