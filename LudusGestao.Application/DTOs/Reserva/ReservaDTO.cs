using LudusGestao.Domain.Enums;
using System;
using LudusGestao.Application.DTOs.Cliente;
using LudusGestao.Application.DTOs.Local;
using LudusGestao.Application.DTOs.Usuario;

namespace LudusGestao.Application.DTOs.Reserva;

public class ReservaDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    // Associações
    public Guid ClienteId { get; set; }
    public ClienteDTO Cliente { get; set; }
    public Guid LocalId { get; set; }
    public LocalDTO Local { get; set; }
    public Guid? UsuarioId { get; set; }
    public UsuarioDTO Usuario { get; set; }
    
    // Dados da reserva
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