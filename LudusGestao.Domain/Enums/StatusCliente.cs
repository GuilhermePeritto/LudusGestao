namespace LudusGestao.Domain.Enums
{
    /// <summary>
    /// Situações para clientes
    /// </summary>
    public enum SituacaoCliente
    {
        Ativo = SituacaoBase.Ativo,
        Inativo = SituacaoBase.Inativo,
        Bloqueado = 3      // Cliente velhaco com títulos vencidos
    }
} 