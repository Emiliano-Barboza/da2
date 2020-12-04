using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.Helper.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NaturalUruguayGateway.Helper.Test
{
    [TestClass]
    public class LodgmentCalculatorTest
    {
        private IFixture fixture = null;
        private const double AdultsDiscountPercentage = 1; 
        private const double UngerAgeDiscountPercentage = 0.5; 
        private const double BabiesDiscountPercentage = 0.25;
        private Lodgment lodgment = null;
        private LodgmentOptionsModel lodgmentOptions = null;
        private long nowDateTimeTicks = 0;
        private int stayDays = 0;
        private double expectedPrice = 0;
        private LodgmentCalculator lodgmentCalculator = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Customize<Lodgment>(c => c
                .Without(x => x.Spot)
                .Without(x => x.Bookings)
                .Without(x => x.Reviews));
            lodgment = fixture.Create<Lodgment>();
            nowDateTimeTicks = DateTime.Now.Ticks;
            stayDays = fixture.Create<int>();
            lodgmentOptions = new LodgmentOptionsModel
            {
                CheckIn = nowDateTimeTicks,
                CheckOut = nowDateTimeTicks + (TimeSpan.TicksPerDay * stayDays),
            };
            lodgmentCalculator = new LodgmentCalculator();
        }
        
        [TestMethod]
        public async Task CalculateTotalStayAsync_RandomAdultsInRandomStayDays_ReturnPriceOk()
        {
            // Arrange
            lodgmentOptions.AmountOfAdults = fixture.Create<byte>();
            expectedPrice = lodgmentOptions.AmountOfAdults * lodgment.Price * AdultsDiscountPercentage * stayDays; 
            
            // Act
            var actualPrice = await lodgmentCalculator.CalculateTotalStayAsync(lodgmentOptions, lodgment);
            
            
            // Assert
            Assert.AreEqual(expectedPrice, actualPrice);
        }
        
        [TestMethod]
        public async Task CalculateTotalStayAsync_RandomUnderAgesInRandomStayDays_ReturnPriceOk()
        {
            // Arrange
            lodgmentOptions.AmountOfUnderAge = fixture.Create<byte>();
            expectedPrice = lodgmentOptions.AmountOfUnderAge * lodgment.Price * UngerAgeDiscountPercentage * stayDays; 
            
            // Act
            var actualPrice = await lodgmentCalculator.CalculateTotalStayAsync(lodgmentOptions, lodgment);
            
            
            // Assert
            Assert.AreEqual(expectedPrice, actualPrice);
        }
        
        [TestMethod]
        public async Task CalculateTotalStayAsync_RandomBabiesInRandomStayDays_ReturnPriceOk()
        {
            // Arrange
            lodgmentOptions.AmountOfBabies = fixture.Create<byte>();
            expectedPrice = lodgmentOptions.AmountOfBabies * lodgment.Price * BabiesDiscountPercentage * stayDays; 
            
            // Act
            var actualPrice = await lodgmentCalculator.CalculateTotalStayAsync(lodgmentOptions, lodgment);
            
            
            // Assert
            Assert.AreEqual(expectedPrice, actualPrice);
        }
        
        [TestMethod]
        public async Task CalculateTotalStayAsync_RandomBabiesUnderAgesAndAdultsInRandomStayDays_ReturnPriceOk()
        {
            // Arrange
            lodgmentOptions.AmountOfUnderAge = fixture.Create<byte>();
            lodgmentOptions.AmountOfBabies = fixture.Create<byte>();
            lodgmentOptions.AmountOfAdults = fixture.Create<byte>();
            var underAgePrice= lodgmentOptions.AmountOfUnderAge * lodgment.Price * UngerAgeDiscountPercentage * stayDays;
            var babyPrice = lodgmentOptions.AmountOfBabies * lodgment.Price * BabiesDiscountPercentage * stayDays; 
            var adultPrice = lodgmentOptions.AmountOfAdults * lodgment.Price * stayDays;
            expectedPrice = underAgePrice + babyPrice + adultPrice;
            
            // Act
            var actualPrice = await lodgmentCalculator.CalculateTotalStayAsync(lodgmentOptions, lodgment);
            
            
            // Assert
            Assert.AreEqual(expectedPrice, actualPrice);
        }
        
        [TestMethod]
        public async Task CalculateTotalStayAsync_NoGuests_ReturnPriceOk()
        {
            // Arrange
            lodgmentOptions.AmountOfUnderAge = 0;
            lodgmentOptions.AmountOfBabies = 0;
            lodgmentOptions.AmountOfAdults = 0;
            expectedPrice = 1 * lodgment.Price * AdultsDiscountPercentage * stayDays;
            
            // Act
            var actualPrice = await lodgmentCalculator.CalculateTotalStayAsync(lodgmentOptions, lodgment);
            
            
            // Assert
            Assert.AreEqual(expectedPrice, actualPrice);
        }
    }
}