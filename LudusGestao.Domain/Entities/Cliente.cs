using LudusGestao.Domain.Entities.Base;
using LudusGestao.Domain.Enums;
using LudusGestao.Domain.ValueObjects;
using System;

namespace LudusGestao.Domain.Entities
{
    public class Cliente : BaseEntity, ITenantEntity
    {
        public string Subtitulo { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Observacoes { get; set; }
        public SituacaoCliente Situacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public int TenantId { get; set; }
    }
} 