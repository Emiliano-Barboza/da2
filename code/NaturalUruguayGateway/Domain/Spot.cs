using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NaturalUruguayGateway.Domain
{
    [Serializable]
    public class Spot
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ICollection<Lodgment> Lodgments { get; set; }
        [JsonIgnore]
        public virtual Region Region { get; set; }
        [Required]
        public virtual int RegionId { get; set; }
        
        public virtual ICollection<CategorySpot> CategorySpots { get; set; }
    }
}