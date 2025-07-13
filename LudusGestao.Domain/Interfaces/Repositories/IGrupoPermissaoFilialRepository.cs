using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories.Base;

namespace LudusGestao.Domain.Interfaces.Repositories
{
    public interface IGrupoPermissaoFilialRepository : IBaseRepository<GrupoPermissaoFilial>
    {
        Task<IEnumerable<GrupoPermissaoFilial>> ObterPorGrupo(Guid grupoId);
        Task<IEnumerable<GrupoPermissaoFilial>> ObterPorFilial(Guid filialId);
        Task<GrupoPermissaoFilial?> ObterPorGrupoEFilial(Guid grupoId, Guid filialId);
    }
} 