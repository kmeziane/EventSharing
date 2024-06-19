using System.ComponentModel.DataAnnotations;

namespace EventSharing.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public  string? Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
