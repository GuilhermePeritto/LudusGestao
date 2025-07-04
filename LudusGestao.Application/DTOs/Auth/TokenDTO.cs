// Arquivo criado para padronização da estrutura de DTOs 

namespace LudusGestao.Application.DTOs.Auth;

public class TokenDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiraEm { get; set; }
} 