using LudusGestao.Domain.Entities.Base;
using LudusGestao.Domain.Enums;
using LudusGestao.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace LudusGestao.Domain.Entities
{
    public class Empresa : BaseEntity, ITenantEntity
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public Email Email { get; set; }
        public Endereco Endereco { get; set; }
        public ICollection<Filial> Filiais { get; set; }
        public int TenantId { get; set; }
        public SituacaoBase Situacao { get; set; }
    }
} 