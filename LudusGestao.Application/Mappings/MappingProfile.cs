using AutoMapper;
using LudusGestao.Application.DTOs.Reserva;
using LudusGestao.Application.DTOs.Cliente;
using LudusGestao.Application.DTOs.Filial;
using LudusGestao.Application.DTOs.Local;
using LudusGestao.Application.DTOs.Recebivel;
using LudusGestao.Application.DTOs.Usuario;
using LudusGestao.Domain.Entities;
using LudusGestao.Application.DTOs.Empresa;
using LudusGestao.Application.DTOs.GrupoPermissao;
using LudusGestao.Application.DTOs.Permissao;

namespace LudusGestao.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Reserva
            CreateMap<CreateReservaDTO, Reserva>();
            CreateMap<UpdateReservaDTO, Reserva>();
            CreateMap<Reserva, ReservaDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao.ToString()));

            // Cliente
            CreateMap<CreateClienteDTO, Cliente>();
            CreateMap<UpdateClienteDTO, Cliente>();
            CreateMap<Cliente, ClienteDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao.ToString()));

            // Filial
            CreateMap<CreateFilialDTO, Filial>();
            CreateMap<UpdateFilialDTO, Filial>();
            CreateMap<Filial, FilialDTO>();

            // Local
            CreateMap<CreateLocalDTO, Local>();
            CreateMap<UpdateLocalDTO, Local>();
            CreateMap<Local, LocalDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao.ToString()));

            // Recebivel
            CreateMap<CreateRecebivelDTO, Recebivel>();
            CreateMap<UpdateRecebivelDTO, Recebivel>();
            CreateMap<Recebivel, RecebivelDTO>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao.ToString()));

            // Usuario
            CreateMap<CreateUsuarioDTO, Usuario>()
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.Senha));
            CreateMap<UpdateUsuarioDTO, Usuario>()
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.Senha));
            CreateMap<Usuario, UsuarioDTO>();

            // Empresa
            CreateMap<CreateEmpresaDTO, Empresa>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => $"{src.Rua}, {src.Numero}"))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Cidade))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.CEP));
            CreateMap<UpdateEmpresaDTO, Empresa>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => $"{src.Rua}, {src.Numero}"))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Cidade))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.CEP));
            CreateMap<Empresa, EmpresaDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Rua, opt => opt.MapFrom(src => src.Endereco))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Cidade))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.CEP, opt => opt.MapFrom(src => src.Cep));

            // GrupoPermissao
            CreateMap<CreateGrupoPermissaoDTO, GrupoPermissao>();
            CreateMap<UpdateGrupoPermissaoDTO, GrupoPermissao>();
            CreateMap<GrupoPermissao, GrupoPermissaoDTO>();

            // Permissao
            CreateMap<Permissao, PermissaoDTO>();
        }
    }
} 