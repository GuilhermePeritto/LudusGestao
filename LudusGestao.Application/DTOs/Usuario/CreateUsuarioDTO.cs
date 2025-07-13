using LudusGestao.Domain.Enums;
using System;

namespace LudusGestao.Application.DTOs.Usuario;

public class CreateUsuarioDTO
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Cargo { get; set; }
    public Guid EmpresaId { get; set; }
    public Guid? GrupoPermissaoId { get; set; }
    public SituacaoBase Situacao { get; set; } = SituacaoBase.Ativo;
    public string? Foto { get; set; }
    public string Senha { get; set; }
} 