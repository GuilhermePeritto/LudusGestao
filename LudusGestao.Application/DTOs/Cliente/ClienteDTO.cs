using System;

namespace LudusGestao.Application.DTOs.Cliente;

public class ClienteDTO
{
    public string Subtitulo { get; set; }
    public string Nome { get; set; }
    public string Documento { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Endereco { get; set; }
    public string Observacoes { get; set; }
    public string Situacao { get; set; }
    public DateTime DataCadastro { get; set; }
    public Guid Id { get; set; }
} 