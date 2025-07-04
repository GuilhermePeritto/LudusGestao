using System;

namespace LudusGestao.Domain.Interfaces.Services
{
    public interface ITenantService
    {
        void SetTenant(string tenantId);
        int GetTenantId();
    }
} 