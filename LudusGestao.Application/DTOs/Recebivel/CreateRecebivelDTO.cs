// Arquivo criado para padronização da estrutura de DTOs 

namespace LudusGestao.Application.DTOs.Recebivel;
using System;
using LudusGestao.Domain.Enums;

public class CreateRecebivelDTO
{
    public Guid ClienteId { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public SituacaoRecebivel Situacao { get; set; }
    public Guid? ReservaId { get; set; }
} 