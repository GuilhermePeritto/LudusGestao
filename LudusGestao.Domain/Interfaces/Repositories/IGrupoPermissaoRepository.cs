using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories.Base;

namespace LudusGestao.Domain.Interfaces.Repositories
{
    public interface IGrupoPermissaoRepository : IBaseRepository<GrupoPermissao>
    {
        Task<IEnumerable<GrupoPermissao>> ObterComUsuarios();
        Task<GrupoPermissao?> ObterComUsuariosPorId(Guid id);
    }
} 