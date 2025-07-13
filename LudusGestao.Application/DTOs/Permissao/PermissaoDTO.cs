using LudusGestao.Domain.Enums;
using System;

namespace LudusGestao.Application.DTOs.Permissao
{
    public class PermissaoDTO
    {
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ModuloPai { get; set; }
        public string Submodulo { get; set; }
        public string Acao { get; set; }
        public SituacaoBase Situacao { get; set; }
        public int TenantId { get; set; }
    }
} 