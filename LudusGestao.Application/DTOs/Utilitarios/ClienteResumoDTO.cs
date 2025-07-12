namespace LudusGestao.Application.DTOs.Utilitarios;

public class ClienteResumoDTO
{
    public int TotalClientes { get; set; }
    public int Ativos { get; set; }
    public int Inativos { get; set; }
    public int NovosMes { get; set; }
    public int PessoaJuridica { get; set; }
} 