using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NaturalUruguayGateway.Domain
{
    [Serializable]
    public class Lodgment
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public byte AmountOfStars { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public ICollection<string> Images { get; set; }
        [Required]
        public double Price { get; set; }
        [NotMapped]
        public double TotalPrice { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string ContactInformation { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
        [JsonIgnore]
        public virtual Spot Spot { get; set; }
        [Required]
        public virtual int SpotId { get; set; }
        
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}