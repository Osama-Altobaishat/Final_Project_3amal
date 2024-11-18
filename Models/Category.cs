using System.ComponentModel.DataAnnotations;

namespace Final_Project_3amal.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Icon { get; set; }

        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
