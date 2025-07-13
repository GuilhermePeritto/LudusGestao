using LudusGestao.Domain.Enums;
using System;

namespace LudusGestao.Application.DTOs.Cliente;

public class UpdateClienteDTO
{
    public string Nome { get; set; }
    public string Documento { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Endereco { get; set; }
    public string Observacoes { get; set; }
    public SituacaoCliente Situacao { get; set; }
} 