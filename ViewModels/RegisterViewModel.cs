using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models{
    public class RegisterViewModel{
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [MinLengthAttribute(2)]
        public string FirstName {get;set;}
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [MinLengthAttribute(2)]
        public string LastName {get;set;}
        
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        public string PasswordConfirmation { get; set; }
    }
}