using System;
using System.ComponentModel.DataAnnotations;

namespace Final_Project_3amal.Models
{
    public enum UserRole
    {
        Admin,
        User,
        Service_Provider
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Birthdate is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        //[DataType(DataType.Upload)]
        //public byte[] Image { get; set; }
        [Required]
        public string Imagebath { get; set; }
        [Required(ErrorMessage = "AboutUs is required")]
        [StringLength(500, ErrorMessage = "AboutUs cannot be longer than 500 characters")]
        public string AboutUs { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot be longer than 200 characters")]
        public string Location { get; set; }

        //public User()
        //{
        //    // Assign a default image byte array (e.g., a default profile picture)
        //    Image = GetDefaultImage();
        //}

        //private byte[] GetDefaultImage()
        //{
        //    // You can load the default image from a file or an embedded resource
        //    // Example: load a default image file from disk
        //    return File.ReadAllBytes("wwwroot/Images/Default.jpg");
        //}

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Service> Services { get; set; } = new List<Service>();   
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}
