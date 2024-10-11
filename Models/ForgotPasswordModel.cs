using System.ComponentModel.DataAnnotations;

namespace GloboClimaAPI.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
