using System.ComponentModel.DataAnnotations;

namespace NaturalUruguayGateway.Domain
{
    public class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string ConfirmationCode { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public byte AmountOfStars { get; set; }
        public virtual Lodgment Lodgment { get; set; }
        public virtual int LodgmentId { get; set; }
    }
}