using System;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.HelperInterface.Services;

namespace NaturalUruguayGateway.Helper.Services
{
    public class LodgmentCalculator : ILodgmentCalculator
    {
        private const int DefaultAmountOfAdults = 1;
        private const double NoDiscountPercentage = 1;
        private const double AdultsDiscountPercentage = 1;
        private const double UngerAgeDiscountPercentage = 0.5;
        private const double BabiesDiscountPercentage = 0.25;
        private const double VeteransDiscountPercentage = 0.7;
        
        private void ValidateLodgmentDatesRange(LodgmentOptionsModel lodgmentOptionsModel)
        {
            if (lodgmentOptionsModel.CheckIn == 0 || lodgmentOptionsModel.CheckOut == 0)
            {
                throw new ArgumentException("CheckIn and CheckOut date must be defined");
            }

            if (lodgmentOptionsModel.CheckOut < lodgmentOptionsModel.CheckIn)
            {
                throw new ArgumentException("CheckOut date can't be earlier than CheckIn date");
            }
        }
        
        private double GetTotalPrice(int amountPersons, double price, double discountPercentage)
        {
            var totalPrice = amountPersons * price  * discountPercentage;
            return totalPrice;
        }

        private bool HasGuests(LodgmentOptionsModel lodgmentOptionsModel)
        {
            var hasGuests = false;
            if (lodgmentOptionsModel != null)
            {
                hasGuests = lodgmentOptionsModel.TotalGuests > 0;
            }

            return hasGuests;
        }

        private double CalculatePriceWithoutGuests(double lodgmentPrice)
        {
            var price = GetTotalPrice(DefaultAmountOfAdults, lodgmentPrice, AdultsDiscountPercentage);
            return price;
        }

        private byte GetEligibleVeteransAmount(LodgmentOptionsModel lodgmentOptionsModel)
        {
            var amountOfVeterans = Convert.ToByte(Math.Floor((decimal) (lodgmentOptionsModel.AmountOfVeterans / 2)));
            return amountOfVeterans;
        }
        
        private double CalculatePriceForVeterans(LodgmentOptionsModel lodgmentOptionsModel, double lodgmentPrice)
        {
            double price = 0;
            if (lodgmentOptionsModel.AmountOfVeterans > 0)
            {
                var eligibleVeterans = GetEligibleVeteransAmount(lodgmentOptionsModel);
                if (eligibleVeterans > 0)
                {
                    price += GetTotalPrice(eligibleVeterans, lodgmentPrice, VeteransDiscountPercentage);
                }

                var nonEligibleVeterans = lodgmentOptionsModel.AmountOfVeterans - eligibleVeterans;
                price += GetTotalPrice(nonEligibleVeterans, lodgmentPrice, NoDiscountPercentage);
            }
            
            return price;
        }
        
        private double CalculatePriceByGuests(LodgmentOptionsModel lodgmentOptionsModel, double lodgmentPrice)
        {
            var price = GetTotalPrice(lodgmentOptionsModel.AmountOfAdults, lodgmentPrice, AdultsDiscountPercentage);
            price += GetTotalPrice(lodgmentOptionsModel.AmountOfUnderAge, lodgmentPrice, UngerAgeDiscountPercentage);
            price += GetTotalPrice(lodgmentOptionsModel.AmountOfBabies, lodgmentPrice, BabiesDiscountPercentage);
            price += CalculatePriceForVeterans(lodgmentOptionsModel, lodgmentPrice);
            return price;
        }
        
        public async Task<double> CalculateTotalStayAsync(LodgmentOptionsModel lodgmentOptionsModel, Lodgment lodgment)
        {
            await Task.Yield();
            double totalPrice = 0;
            if (lodgmentOptionsModel != null)
            {
                ValidateLodgmentDatesRange(lodgmentOptionsModel);
                var timeSpan = new TimeSpan(lodgmentOptionsModel.CheckOut - lodgmentOptionsModel.CheckIn);
                double price = 0;
            
                if (HasGuests(lodgmentOptionsModel))
                {
                    price = CalculatePriceByGuests(lodgmentOptionsModel, lodgment.Price);
                }
                else
                {
                    price = CalculatePriceWithoutGuests(lodgment.Price);
                }
            
                totalPrice = price * Math.Floor(timeSpan.TotalDays);
            }
            
            return totalPrice;
        }
        
    }
}