using LudusGestao.Domain.Enums;
using System;

namespace LudusGestao.Application.DTOs.Reserva;

public class CreateReservaDTO
{
    public Guid ClienteId { get; set; }
    public Guid LocalId { get; set; }
    public Guid? UsuarioId { get; set; }
    public DateTime Data { get; set; }
    public string HoraInicio { get; set; }
    public string HoraFim { get; set; }
    public SituacaoReserva Situacao { get; set; }
    public string Cor { get; set; }
    public string Esporte { get; set; }
    public string Observacoes { get; set; }
    public decimal Valor { get; set; }
} 