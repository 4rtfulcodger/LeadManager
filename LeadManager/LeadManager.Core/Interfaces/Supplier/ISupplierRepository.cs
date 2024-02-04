using LeadManager.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Interfaces.Supplier
{
    public interface ISupplierRepository
    {
        Task<bool> AddSupplierAsync(LeadManager.Core.Entities.Supplier.Supplier supplier);
        Task<bool> UpdateSupplierAsync(int Id);
        Task<PagedList<LeadManager.Core.Entities.Supplier.Supplier>> GetSuppliersAsync(SupplierFilter filter);
        Task<LeadManager.Core.Entities.Supplier.Supplier?> GetSupplierWithIdAsync(int Id);
        Task<LeadManager.Core.Entities.Supplier.Supplier?> GetSupplierWithRefAsync(Guid supplierRef);
        Task<bool> DeleteSupplier(LeadManager.Core.Entities.Supplier.Supplier supplier);
    }
}
