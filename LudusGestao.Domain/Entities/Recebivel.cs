using LudusGestao.Domain.Entities.Base;
using System;

namespace LudusGestao.Domain.Entities
{
    public class Recebivel : BaseEntity, ITenantEntity
    {
        public string Cliente { get; set; }
        public Guid ClienteId { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public SituacaoRecebivel Situacao { get; set; }
        public Guid? ReservaId { get; set; }
        public DateTime DataCadastro { get; set; }
        public int TenantId { get; set; }
    }
    public enum SituacaoRecebivel { Pendente, Pago, Vencido }
} 