using System;

namespace LudusGestao.Domain.Entities.Base
{
    public interface IAuditableEntity
    {
        DateTime DataCriacao { get; set; }
        DateTime? DataAtualizacao { get; set; }
    }
} 