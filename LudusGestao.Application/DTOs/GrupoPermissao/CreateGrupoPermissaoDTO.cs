using LudusGestao.Domain.Enums;

namespace LudusGestao.Application.DTOs.GrupoPermissao
{
    public class CreateGrupoPermissaoDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public SituacaoBase Situacao { get; set; } = SituacaoBase.Ativo;
    }
} 