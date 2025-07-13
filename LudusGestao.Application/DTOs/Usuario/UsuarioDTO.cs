namespace LudusGestao.Application.DTOs.Usuario;
using System;
using System.Collections.Generic;
using LudusGestao.Domain.Enums;

public class UsuarioDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public SituacaoBase Situacao { get; set; }
    public int TenantId { get; set; }
    public string Telefone { get; set; }
    public string Cargo { get; set; }
    public Guid EmpresaId { get; set; }
    public Guid? GrupoPermissaoId { get; set; }
    public DateTime UltimoAcesso { get; set; }
    public string? Foto { get; set; }
} 