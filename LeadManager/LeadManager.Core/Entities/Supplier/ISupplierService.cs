using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Entities.Supplier
{
    public interface ISupplierService
    {
        Task<bool> AddSupplierAsync(Supplier supplier);
        Task<bool> UpdateSupplierAsync(int Id);
        Task<IEnumerable<Supplier>> GetSuppliersAsync();
        Task<Supplier?> GetSupplierWithIdAsync(int Id);
        Task<bool> DeleteSupplier(Supplier supplier);
    }
}
