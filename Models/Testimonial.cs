using System.ComponentModel.DataAnnotations;

namespace Final_Project_3amal.Models
{
    public class Testimonial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public decimal Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [Required]
        public bool Acsept { get; set; }

        public User User { get; set; }
    }
}
