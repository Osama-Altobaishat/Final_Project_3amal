using System.ComponentModel.DataAnnotations;

namespace Final_Project_3amal.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Range(0, 5, ErrorMessage = "Communication rating must be between 1 and 5.")]
        public decimal RatingCommunicator { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public Service Service { get; set; }
        public User User { get; set; }
    }
}
