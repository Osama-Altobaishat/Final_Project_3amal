using System.ComponentModel.DataAnnotations;

namespace Final_Project_3amal.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 5, ErrorMessage = "Average rating must be between 1 and 5.")]
        public decimal RatingAvg { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
        public ICollection<ImageService> ImageService { get; set; } = new List<ImageService>();

    }
}
