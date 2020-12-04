using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NaturalUruguayGateway.Domain
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Spot> Spots { get; set; }
    }
}