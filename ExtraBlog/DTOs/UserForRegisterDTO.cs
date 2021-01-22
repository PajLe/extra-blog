using System.ComponentModel.DataAnnotations;

namespace ExtraBlog.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "You must specify a user that is at least 4 characters long!")]
        public string Username { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "You must specify a password that is at least 4 characters long!")]
        public string Password { get; set; }
    }
}
