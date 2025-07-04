using LudusGestao.Domain.Enums;
using System;

namespace LudusGestao.Application.DTOs.Empresa
{
    public class UpdateEmpresaDTO
    {
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public SituacaoBase Situacao { get; set; }
    }
} 