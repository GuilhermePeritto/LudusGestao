using LudusGestao.Domain.Entities.Base;
using System;

namespace LudusGestao.Domain.Entities
{
    public class Filial : BaseEntity, ITenantEntity
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }
        public string Responsavel { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAbertura { get; set; }
        public int TenantId { get; set; }
        public Empresa Empresa { get; set; }
    }
} 