using LudusGestao.Domain.Entities.Base;
using LudusGestao.Domain.Enums;
using System;

namespace LudusGestao.Domain.Entities
{
    public class GrupoPermissao : BaseEntity, ITenantEntity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public SituacaoBase Situacao { get; set; }
        public int TenantId { get; set; }
    }
} 