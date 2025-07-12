using LudusGestao.Application.DTOs.Auth;
using LudusGestao.Application.Common.Models;
using System.Threading.Tasks;

namespace LudusGestao.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<TokenDTO>> LoginAsync(LoginDTO dto);
        Task<ApiResponse<TokenDTO>> RefreshAsync(RefreshTokenDTO dto);
    }
} 