using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Account
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
