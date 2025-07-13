// Arquivo criado para padronização da estrutura de DTOs 

namespace LudusGestao.Application.DTOs.Local;
using System;
using System.Collections.Generic;
using LudusGestao.Domain.Enums;
using LudusGestao.Application.DTOs.Filial;

public class LocalDTO
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public int Intervalo { get; set; }
    public decimal ValorHora { get; set; }
    public int? Capacidade { get; set; }
    public string Descricao { get; set; }
    public List<string> Comodidades { get; set; } = new List<string>();
    public SituacaoLocal Situacao { get; set; }
    public string Cor { get; set; }
    public string HoraAbertura { get; set; }
    public string HoraFechamento { get; set; }
    
    // Associações
    public Guid FilialId { get; set; }
    public FilialDTO Filial { get; set; }
    
    public int TenantId { get; set; }
} 