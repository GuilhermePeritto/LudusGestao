// Arquivo criado para padronização da estrutura de DTOs 

namespace LudusGestao.Application.DTOs.Filial;
using System;
using LudusGestao.Domain.Enums;

public class UpdateFilialDTO
{
    public string Nome { get; set; }
    public string Codigo { get; set; }
    public string Endereco { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Cep { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string Cnpj { get; set; }
    public string Responsavel { get; set; }
    public SituacaoBase Situacao { get; set; }
    public DateTime DataAbertura { get; set; }
    public Guid EmpresaId { get; set; }
} 