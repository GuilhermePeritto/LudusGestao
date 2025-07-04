using AutoMapper;
using LudusGestao.Application.DTOs.Empresa;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories.Base;
using LudusGestao.Application.Common.Interfaces;
using LudusGestao.Application.Common.Services;
using LudusGestao.Domain.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LudusGestao.Application.Services
{
    public class EmpresaService : BaseCrudService<Empresa, EmpresaDTO, CreateEmpresaDTO, UpdateEmpresaDTO>, IBaseCrudService<EmpresaDTO, CreateEmpresaDTO, UpdateEmpresaDTO>
    {
        public EmpresaService(IBaseRepository<Empresa> repository, IMapper mapper) : base(repository, mapper) { }
        
        // Adicione aqui apenas métodos customizados que não sejam CRUD padrão
    }
} 