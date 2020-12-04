namespace NaturalUruguayGateway.Domain
{
    public class CategorySpot
    {
        public int SpotId { get; set; }
        public Spot Spot { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}