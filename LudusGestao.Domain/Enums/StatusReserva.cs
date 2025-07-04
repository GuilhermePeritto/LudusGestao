namespace LudusGestao.Domain.Enums
{
    /// <summary>
    /// Situações para reservas/agenda
    /// </summary>
    public enum SituacaoReserva
    {
        Confirmado = 1,    // Cliente pagou
        Concluido = 2,     // Pós término do evento
        Pendente = 3,      // Confirmação pendente (ainda não pagou)
        Cancelado = 4      // Cancelado
    }
} 