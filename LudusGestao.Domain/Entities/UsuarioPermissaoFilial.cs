using LudusGestao.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace LudusGestao.Domain.Entities
{
    public class UsuarioPermissaoFilial : BaseEntity, ITenantEntity
    {
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public Guid FilialId { get; set; }
        public Filial Filial { get; set; }
        public List<Guid> Permissoes { get; set; } = new List<Guid>();
        public int TenantId { get; set; }
    }
} 