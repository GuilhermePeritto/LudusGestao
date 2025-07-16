using LudusGestao.Application.Common.Models;
using LudusGestao.Application.DTOs.Gerencialmento;
using System.Threading.Tasks;

namespace LudusGestao.Application.Common.Interfaces
{
    public interface IGerencialmentoService
    {
        Task<ApiResponse<NovoClienteResultadoDTO>> CriarNovoCliente(CriarNovoClienteDTO dto);
        Task<ApiResponse<object>> AlterarSenha(AlterarSenhaDTO dto);
    }
} 