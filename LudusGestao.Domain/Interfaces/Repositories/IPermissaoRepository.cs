using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories.Base;

namespace LudusGestao.Domain.Interfaces.Repositories
{
    public interface IPermissaoRepository : IBaseRepository<Permissao>
    {
        Task<IEnumerable<Permissao>> ObterPorModuloPai(string moduloPai);
        Task<IEnumerable<Permissao>> ObterPorSubmodulo(string submodulo);
        Task<IEnumerable<string>> ObterModulosPai();
        Task<IEnumerable<string>> ObterSubmodulos();
    }
} 