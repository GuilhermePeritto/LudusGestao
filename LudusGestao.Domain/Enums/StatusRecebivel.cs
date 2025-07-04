namespace LudusGestao.Domain.Enums
{
    /// <summary>
    /// Situações para recebíveis
    /// </summary>
    public enum SituacaoRecebivel
    {
        Aberto = 1,        // Título ainda não pago
        Vencido = 2,       // Passou da data de vencimento
        Pago = 3,          // Pago
        Cancelado = 4,     // Cancelou evento ou cancelado manualmente
        Estornado = 5      // Somente manualmente
    }
} 