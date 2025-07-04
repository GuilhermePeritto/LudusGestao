using System;
using System.Threading.Tasks;
using AutoMapper;
using LudusGestao.Application.DTOs.Usuario;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Application.Common.Interfaces;
using LudusGestao.Application.Common.Services;
using LudusGestao.Domain.Common;
using LudusGestao.Application.Common.Models;

namespace LudusGestao.Application.Services
{
    public class UsuarioService : BaseCrudService<Usuario, UsuarioDTO, CreateUsuarioDTO, UpdateUsuarioDTO>, IBaseCrudService<UsuarioDTO, CreateUsuarioDTO, UpdateUsuarioDTO>
    {
        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper) : base(usuarioRepository, mapper) { }
    }
}
