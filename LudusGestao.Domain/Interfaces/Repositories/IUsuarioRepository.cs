using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories.Base;

namespace LudusGestao.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario?> ObterPorEmail(string email);
        Task<Usuario?> ObterPorEmailGlobal(string email);
        Task<IEnumerable<Usuario>> ObterPorEmpresa(Guid empresaId);
        Task<IEnumerable<Usuario>> ObterPorGrupo(Guid grupoId);
    }
} 