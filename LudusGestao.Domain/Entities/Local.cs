using LudusGestao.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace LudusGestao.Domain.Entities
{
    public class Local : BaseEntity, ITenantEntity
    {
        public string Subtitulo { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public int Intervalo { get; set; }
        public decimal ValorHora { get; set; }
        public int? Capacidade { get; set; }
        public string Descricao { get; set; }
        public List<string> Comodidades { get; set; } = new List<string>();
        public SituacaoLocal Situacao { get; set; }
        public string Cor { get; set; }
        public string HoraAbertura { get; set; }
        public string HoraFechamento { get; set; }
        public Guid FilialId { get; set; }
        public Filial Filial { get; set; }
        public int TenantId { get; set; }
        public DateTime DataCadastro { get; set; }
    }
    public enum SituacaoLocal { Ativo, Inativo, Manutencao }
} 