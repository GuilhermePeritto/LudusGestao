using LudusGestao.Application.DTOs.Empresa;
using LudusGestao.Application.DTOs.Filial;
using LudusGestao.Application.DTOs.Usuario;

namespace LudusGestao.Application.DTOs.Gerencialmento
{
    public class NovoClienteResultadoDTO
    {
        public int TenantId { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public FilialDTO FilialMatriz { get; set; }
        public UsuarioDTO UsuarioAdmin { get; set; }
        public string SenhaPadrao { get; set; }
    }
} 