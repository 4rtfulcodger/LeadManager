using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Supplier;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Infrastructure.Data.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly LeadManagerDbContext _dbContext;

        public SupplierRepository(LeadManagerDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> AddSupplierAsync(Supplier supplier)
        {
            _dbContext.Add(supplier);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdateSupplierAsync(int Id)
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            return await _dbContext.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetSupplierWithIdAsync(int Id)
        {
            return await _dbContext.Suppliers.Where(l => l.SupplierId == Id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteSupplier(Supplier supplier)
        {
            _dbContext.Remove(supplier);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }
    }
}
