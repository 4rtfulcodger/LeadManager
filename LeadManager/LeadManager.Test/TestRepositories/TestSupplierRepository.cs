using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Test.TestRepositories
{
    public class TestSupplierRepository: ISupplierRepository
    {
        static List<Supplier> supplierList = new List<Supplier>();

        public TestSupplierRepository()
        {
                
        }

        public async Task<bool> AddSupplierAsync(Supplier supplier)
        {
            supplier.SupplierId = supplierList.Count + 1;
            supplierList.Add(supplier);
            return true;
        }

        public async Task<bool> DeleteSupplier(Supplier supplier)
        {
            await Task.Delay(0);
            return true;
        }

        public async Task<PagedList<Supplier>> GetSuppliersAsync(SupplierFilter filter)
        {
            IQueryable<Supplier> queryableSupplierList = supplierList.AsQueryable();
            return await PagedList<Supplier>.Create(queryableSupplierList, filter.PageNumber, filter.PageSize);
        }

        public async Task<Supplier?> GetSupplierWithIdAsync(int Id)
        {
            await Task.Delay(0);
            return supplierList.FirstOrDefault(sl => sl.SupplierId == Id);
        }

        public async Task<bool> UpdateSupplierAsync(int Id)
        {
            await Task.Delay(0);
            return true;
        }
    }
}
