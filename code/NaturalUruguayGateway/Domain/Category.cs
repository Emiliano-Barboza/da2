using System.Collections.Generic;

namespace NaturalUruguayGateway.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<CategorySpot> CategorySpots { get; set; }
    }
}