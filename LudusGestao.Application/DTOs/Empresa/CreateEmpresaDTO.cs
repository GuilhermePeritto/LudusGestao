namespace LudusGestao.Application.DTOs.Empresa
{
    public class CreateEmpresaDTO
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public Guid Id { get; set; }
    }
} 