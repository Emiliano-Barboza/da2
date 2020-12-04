using System;

namespace NaturalUruguayGateway.Domain.Models
{
    public class LodgmentOptionsModel
    {
        public long CheckIn { get; set; }
        public long CheckOut { get; set; }
        public byte AmountOfAdults { get; set; }
        public byte AmountOfUnderAge { get; set; }
        public byte AmountOfBabies { get; set; }
        public byte AmountOfVeterans { get; set; }

        public int TotalGuests
        {
            get
            {
                var totalOfGuests = AmountOfAdults + AmountOfBabies + AmountOfVeterans + AmountOfUnderAge;
                return totalOfGuests;
            }
        }
    }
}