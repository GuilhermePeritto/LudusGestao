namespace LudusGestao.Application.DTOs.Utilitarios;

public class RecebivelResumoDTO
{
    public decimal ValorTotal { get; set; }
    public decimal ValorPendente { get; set; }
    public decimal ValorPago { get; set; }
    public decimal ValorVencido { get; set; }
    public int TotalRecebiveis { get; set; }
    public int Pendentes { get; set; }
    public int Pagos { get; set; }
    public int Vencidos { get; set; }
} 