namespace LudusGestao.Domain.Interfaces.Services
{
    public interface ISeedService
    {
        Task<bool> SeedDadosBaseAsync();
    }
} 