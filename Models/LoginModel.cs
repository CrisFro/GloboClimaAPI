using System.ComponentModel.DataAnnotations;

namespace GloboClimaAPI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome de usuário não pode exceder 50 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Password { get; set; }
    }
}
