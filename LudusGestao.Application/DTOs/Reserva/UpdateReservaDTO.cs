namespace LudusGestao.Application.DTOs.Reserva;
using System;

public class UpdateReservaDTO
{
    public string Cliente { get; set; }
    public string Local { get; set; }
    public DateTime Data { get; set; }
    public string HoraInicio { get; set; }
    public string HoraFim { get; set; }
    public string Situacao { get; set; }
    public string Cor { get; set; }
    public string Esporte { get; set; }
    public string Observacoes { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataCadastro { get; set; }
    public Guid ClienteId { get; set; }
    public Guid LocalId { get; set; }
} 