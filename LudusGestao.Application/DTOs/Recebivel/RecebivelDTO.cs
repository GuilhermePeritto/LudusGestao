// Arquivo criado para padronização da estrutura de DTOs 

namespace LudusGestao.Application.DTOs.Recebivel;
using System;
using LudusGestao.Domain.Enums;
using LudusGestao.Application.DTOs.Cliente;
using LudusGestao.Application.DTOs.Reserva;

public class RecebivelDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    // Associações
    public Guid ClienteId { get; set; }
    public ClienteDTO Cliente { get; set; }
    public Guid? ReservaId { get; set; }
    public ReservaDTO Reserva { get; set; }
    
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public SituacaoRecebivel Situacao { get; set; }
    public int TenantId { get; set; }
} 