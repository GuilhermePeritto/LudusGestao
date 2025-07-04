using System;

namespace LudusGestao.Application.DTOs.Empresa
{
    public class EmpresaDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
    }
} 