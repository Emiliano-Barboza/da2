using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NaturalUruguayGateway.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public long CheckIn { get; set; }
        [Required]
        public long CheckOut { get; set; }
        [Required]
        public byte AmountGuest { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string ConfirmationCode { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int BookingStatusId { get; set; }
        public BookingStatus BookingStatus { get; set; }
        [Required]
        public int LodgmentId { get; set; }
        public Lodgment Lodgment { get; set; }
        [Required]
        public string StatusDescription { get; set; }
    }
}