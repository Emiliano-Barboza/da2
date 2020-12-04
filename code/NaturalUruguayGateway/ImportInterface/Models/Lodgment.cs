namespace NaturalUruguayGateway.ImportInterface.Models
{
    public class Lodgment
    {
        public string Name { get; set; }
        public byte AmountOfStars { get; set; }
        public string Address { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactInformation { get; set; }
        public int SpotId { get; set; }
    }
}