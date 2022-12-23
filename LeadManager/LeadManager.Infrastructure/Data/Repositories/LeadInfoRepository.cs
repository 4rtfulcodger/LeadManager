using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Infrastructure.Data.Repositories
{
    public class LeadInfoRepository : ILeadInfoRepository
    {
        private readonly LeadManagerDbContext _dbContext;

        public LeadInfoRepository(LeadManagerDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Leads

        public async Task<bool> AddLeadAsync(Lead lead)
        {
            _dbContext.Add(lead);   

            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<IEnumerable<Lead>> GetLeadsAsync(bool includeSource = false, bool includeSupplier = false)
        {
            IQueryable<Lead> leads = _dbContext.Leads;

            if (includeSource)
                leads = leads.Include(l => l.Source);

            if (includeSupplier)
                leads = leads.Include(l => l.Supplier);
            
            return await leads.ToListAsync();
        }

        public async Task<Lead?> GetLeadWithIdAsync(int Id, bool includeSource = false, bool includeSupplier = false)
        {
            IQueryable<Lead> leads = _dbContext.Leads; 

            if (includeSource)
                leads = leads.Include(l => l.Source);

            if (includeSupplier)
                leads = leads.Include(l => l.Supplier);

            return await leads.Where(l => l.LeadId == Id).FirstOrDefaultAsync();
        }

        #endregion

        #region Source

        public async Task<IEnumerable<Source>> GetSourcesAsync()
        {
            return await _dbContext.Sources.ToListAsync();
        }

        public async Task<Source?> GetSourceWithIdAsync(int Id)
        {
            return await _dbContext.Sources.Where(l => l.SourceId == Id).FirstOrDefaultAsync();
        }

        #endregion

        #region Supplier
        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            return await _dbContext.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetSupplierWithIdAsync(int Id)
        {
            return await _dbContext.Suppliers.Where(l => l.SupplierId == Id).FirstOrDefaultAsync();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
