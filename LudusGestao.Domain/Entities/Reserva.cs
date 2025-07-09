using LudusGestao.Domain.Entities.Base;
using LudusGestao.Domain.Enums;
using System;

namespace LudusGestao.Domain.Entities
{
    public class Reserva : BaseEntity, ITenantEntity
    {
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public Guid LocalId { get; set; }
        public Local Local { get; set; }
        public Guid? UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime Data { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFim { get; set; }
        public SituacaoReserva Situacao { get; set; }
        public string Cor { get; set; }
        public string Esporte { get; set; }
        public string Observacoes { get; set; }
        public decimal Valor { get; set; }
        public int TenantId { get; set; }
    }
} 