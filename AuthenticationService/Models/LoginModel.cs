using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models
{
    /// <summary>
    /// LoginModel class represents the data required for user login.
    /// </summary>
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}