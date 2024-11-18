using System.ComponentModel.DataAnnotations;

namespace Final_Project_3amal.Models
{
    public class ImageService
    {
        [Key]
        public int Id { get; set; }
        public string ImagePath { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
