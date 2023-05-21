using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadManager.Core.Entities.Supplier;

namespace LeadManager.Core.Interfaces.Supplier
{
    public interface ISupplierService
    {
        Task<bool> AddSupplierAsync(LeadManager.Core.Entities.Supplier.Supplier supplier);
        Task<bool> UpdateSupplierAsync(int Id);
        Task<IEnumerable<LeadManager.Core.Entities.Supplier.Supplier>> GetSuppliersAsync();
        Task<LeadManager.Core.Entities.Supplier.Supplier?> GetSupplierWithIdAsync(int Id);
        Task<bool> DeleteSupplier(LeadManager.Core.Entities.Supplier.Supplier supplier);
    }
}
