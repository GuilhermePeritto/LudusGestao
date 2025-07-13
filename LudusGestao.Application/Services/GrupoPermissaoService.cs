using LudusGestao.Application.Common.Interfaces;
using LudusGestao.Application.DTOs.GrupoPermissao;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces.Repositories;
using LudusGestao.Domain.Interfaces.Repositories.Base;
using LudusGestao.Domain.Common;
using LudusGestao.Application.Common.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LudusGestao.Application.Services
{
    public class GrupoPermissaoService : IBaseCrudService<GrupoPermissaoDTO, CreateGrupoPermissaoDTO, UpdateGrupoPermissaoDTO>
    {
        private readonly IGrupoPermissaoRepository _repository;
        private readonly IMapper _mapper;

        public GrupoPermissaoService(IGrupoPermissaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GrupoPermissaoDTO> Criar(CreateGrupoPermissaoDTO dto)
        {
            var entity = _mapper.Map<GrupoPermissao>(dto);
            await _repository.Criar(entity);
            return _mapper.Map<GrupoPermissaoDTO>(entity);
        }

        public async Task<GrupoPermissaoDTO> ObterPorId(Guid id)
        {
            var entity = await _repository.ObterPorId(id);
            return _mapper.Map<GrupoPermissaoDTO>(entity);
        }

        public async Task<IEnumerable<GrupoPermissaoDTO>> Listar()
        {
            var entities = await _repository.Listar();
            return _mapper.Map<IEnumerable<GrupoPermissaoDTO>>(entities);
        }

        public async Task<ApiPagedResponse<GrupoPermissaoDTO>> ListarPaginado(QueryParamsBase queryParams)
        {
            var (items, totalCount) = await _repository.ListarPaginado(queryParams);
            var dtos = _mapper.Map<IEnumerable<GrupoPermissaoDTO>>(items);
            
            return new ApiPagedResponse<GrupoPermissaoDTO>(dtos, queryParams.Page, queryParams.Limit, totalCount);
        }

        public async Task<GrupoPermissaoDTO> Atualizar(Guid id, UpdateGrupoPermissaoDTO dto)
        {
            var entity = await _repository.ObterPorId(id);
            if (entity == null) throw new Domain.Common.Exceptions.NotFoundException("Grupo de permissão não encontrado");

            _mapper.Map(dto, entity);
            await _repository.Atualizar(entity);
            return _mapper.Map<GrupoPermissaoDTO>(entity);
        }

        public async Task<bool> Remover(Guid id)
        {
            var entity = await _repository.ObterPorId(id);
            if (entity == null) return false;

            await _repository.Remover(id);
            return true;
        }
    }
} 