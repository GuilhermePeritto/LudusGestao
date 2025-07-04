// Arquivo criado para padronização da estrutura de DTOs 

namespace LudusGestao.Application.DTOs.Base;

/// <summary>
/// DTO base para herança de outros DTOs.
/// </summary>
public abstract class BaseDTO
{
    public Guid Id { get; set; }
} 