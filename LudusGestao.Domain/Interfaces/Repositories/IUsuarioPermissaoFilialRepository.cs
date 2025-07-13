using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories.Base;

namespace LudusGestao.Domain.Interfaces.Repositories
{
    public interface IUsuarioPermissaoFilialRepository : IBaseRepository<UsuarioPermissaoFilial>
    {
        Task<IEnumerable<UsuarioPermissaoFilial>> ObterPorUsuario(Guid usuarioId);
        Task<IEnumerable<UsuarioPermissaoFilial>> ObterPorFilial(Guid filialId);
        Task<UsuarioPermissaoFilial?> ObterPorUsuarioEFilial(Guid usuarioId, Guid filialId);
        Task<IEnumerable<Guid>> ObterFiliaisDoUsuario(Guid usuarioId);
    }
} 