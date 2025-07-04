using AutoMapper;
using LudusGestao.Application.DTOs.Local;
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
    public class LocalService : BaseCrudService<Local, LocalDTO, CreateLocalDTO, UpdateLocalDTO>, IBaseCrudService<LocalDTO, CreateLocalDTO, UpdateLocalDTO>
    {
        public LocalService(IBaseRepository<Local> repository, IMapper mapper) : base(repository, mapper) { }

        // Adicione aqui apenas métodos customizados que não sejam CRUD padrão
    }
} 