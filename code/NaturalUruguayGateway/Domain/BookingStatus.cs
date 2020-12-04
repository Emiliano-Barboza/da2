using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NaturalUruguayGateway.Domain
{
    public class BookingStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}