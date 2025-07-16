using System.ComponentModel.DataAnnotations;

namespace LudusGestao.Application.DTOs.Gerencialmento
{
    public class AlterarSenhaDTO
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nova senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        public string NovaSenha { get; set; }
    }
} 