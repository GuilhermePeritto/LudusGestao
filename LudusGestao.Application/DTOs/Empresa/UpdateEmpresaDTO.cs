using LudusGestao.Domain.Enums;
using System;

namespace LudusGestao.Application.DTOs.Empresa
{
    public class UpdateEmpresaDTO
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public SituacaoBase Situacao { get; set; }
    }
} 