using System.ComponentModel.DataAnnotations;

namespace Final_Project_3amal.Models
{
    public enum Status
    {
        Pending,
        Accepted,
        Active,
        Rejected,
        Completed
    }
    public class ServiceRequest
    {
        [Key] 
        public int Id { get; set; } 

        [Required] 
        public int ServiceId { get; set; } 

        [Required] 
        public int UserId { get; set; } 

        [Required] 
        [StringLength(100)] 
        public string Title { get; set; } 


        public string Description { get; set; }

        [Required] 
        [StringLength(200)]
        public string Location { get; set; } 

        [Required]
        public decimal lat { get; set; }

        [Required]
        public decimal lng { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; } 

        public Status Status { get; set; }

        [DataType(DataType.DateTime)] 
        [Required]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)] 
        [Required]
        public DateTime FinishDate { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime SendDate { get; set; }

        public Service Service { get; set; }
        public User User { get; set; }
    }
}
