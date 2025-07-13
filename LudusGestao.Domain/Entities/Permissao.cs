using LudusGestao.Domain.Entities.Base;
using LudusGestao.Domain.Enums;
using System;

namespace LudusGestao.Domain.Entities
{
    public class Permissao : BaseEntity, ITenantEntity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ModuloPai { get; set; }
        public string Submodulo { get; set; }
        public string Acao { get; set; }
        public SituacaoBase Situacao { get; set; }
        public int TenantId { get; set; }
    }
} 