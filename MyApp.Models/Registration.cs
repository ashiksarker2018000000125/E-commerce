using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name ")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6)]
        [DisplayName("Password")]
        public string Password { get; set; }



        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

    }
}
