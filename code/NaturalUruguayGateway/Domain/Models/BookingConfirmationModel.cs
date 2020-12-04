using System.ComponentModel.DataAnnotations;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.Domain.Models
{
    public class BookingConfirmationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public LodgmentOptionsModel LodgmentOptions { get; set; }
    }
}