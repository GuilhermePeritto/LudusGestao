using System.ComponentModel.DataAnnotations;

namespace LudusGestao.Application.DTOs.Gerencialmento
{
    public class CriarNovoClienteDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Rua é obrigatória")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "Número é obrigatório")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatória")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Estado é obrigatório")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "CEP é obrigatório")]
        public string CEP { get; set; }
    }
} 