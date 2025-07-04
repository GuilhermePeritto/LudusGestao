using LudusGestao.Domain.Interfaces.Services;
using System;
using System.Threading;

namespace LudusGestao.Infrastructure.Security
{
    public class TenantService : ITenantService
    {
        private static readonly AsyncLocal<int> _tenantId = new AsyncLocal<int>();

        public void SetTenant(string tenantId)
        {
            if (int.TryParse(tenantId, out var id))
                _tenantId.Value = id;
            else
                throw new ArgumentException("TenantId inválido");
        }

        public int GetTenantId()
        {
            if (_tenantId.Value == 0)
                throw new InvalidOperationException("TenantId não definido");
            return _tenantId.Value;
        }
    }
} 