
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }
        public string encryptedContactId { get; set; }

        [Required(ErrorMessage ="First Name Required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
    }
}
