using System.ComponentModel.DataAnnotations;

namespace GloboClimaAPI.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string NewPassword { get; set; }
    }
}
