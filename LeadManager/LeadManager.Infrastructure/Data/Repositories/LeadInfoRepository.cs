using LeadManager.Core.Entities;
using LeadManager.Core.Helpers;
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

        public async Task<bool> UpdateLeadAsync(int leadId)
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<IEnumerable<Lead>> GetLeadsAsync(LeadFilter leadFilter)
        {
            IQueryable<Lead> leads = _dbContext.Leads;

            if (leadFilter.IncludeSource)
                leads = leads.Include(l => l.Source);

            if (leadFilter.IncludeSupplier)
                leads = leads.Include(l => l.Supplier);


            if (leadFilter.supplierIds.Count() > 0)
                leads = leads.Where(l => leadFilter.supplierIds.Contains(l.SupplierId)); 


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

        public async Task<bool> DeleteLead(Lead lead)
        {
           _dbContext.Remove(lead);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        #endregion

        #region Source

        public async Task<bool> AddSourceAsync(Source source)
        {
            _dbContext.Add(source);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdateSourceAsync(int Id)
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<IEnumerable<Source>> GetSourcesAsync()
        {
            return await _dbContext.Sources.ToListAsync();
        }

        public async Task<Source?> GetSourceWithIdAsync(int Id)
        {
            return await _dbContext.Sources.Where(l => l.SourceId == Id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteSource(Source source)
        {
            _dbContext.Remove(source);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        #endregion

        #region Supplier

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

        #endregion

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }        
    }
}
