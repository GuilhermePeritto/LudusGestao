using AutoMapper;
using LudusGestao.Application.DTOs.Reserva;
using LudusGestao.Application.DTOs.Cliente;
using LudusGestao.Application.DTOs.Filial;
using LudusGestao.Application.DTOs.Local;
using LudusGestao.Application.DTOs.Recebivel;
using LudusGestao.Application.DTOs.Usuario;
using LudusGestao.Domain.Entities;
using LudusGestao.Application.DTOs.Empresa;

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
                .ForMember(dest => dest.SenhaHash, opt => opt.MapFrom(src => src.Senha));
            CreateMap<UpdateUsuarioDTO, Usuario>()
                .ForMember(dest => dest.SenhaHash, opt => opt.MapFrom(src => src.Senha));
            CreateMap<Usuario, UsuarioDTO>();

            // Empresa
            CreateMap<CreateEmpresaDTO, Empresa>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new LudusGestao.Domain.ValueObjects.Email(src.Email)))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => new LudusGestao.Domain.ValueObjects.Endereco(src.Rua, src.Numero, src.Bairro, src.Cidade, src.Estado, src.CEP)));
            CreateMap<UpdateEmpresaDTO, Empresa>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new LudusGestao.Domain.ValueObjects.Email(src.Email)))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => new LudusGestao.Domain.ValueObjects.Endereco(src.Rua, src.Numero, src.Bairro, src.Cidade, src.Estado, src.CEP)));
            CreateMap<Empresa, EmpresaDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Endereco))
                .ForMember(dest => dest.Rua, opt => opt.MapFrom(src => src.Endereco.Rua))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Endereco.Numero))
                .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => src.Endereco.Bairro))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Endereco.Cidade))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Endereco.Estado))
                .ForMember(dest => dest.CEP, opt => opt.MapFrom(src => src.Endereco.CEP));
        }
    }
} 