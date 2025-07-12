namespace LudusGestao.Application.DTOs.Utilitarios;

public class LocalResumoDTO
{
    public int TotalLocais { get; set; }
    public int Ativos { get; set; }
    public int Inativos { get; set; }
    public int Manutencao { get; set; }
    public decimal ValorMedioHora { get; set; }
} 