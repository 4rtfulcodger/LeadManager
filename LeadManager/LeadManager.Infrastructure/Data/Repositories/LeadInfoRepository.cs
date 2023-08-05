using LeadManager.Core.Entities;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Lead;
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

        public async Task<bool> CreateLeadTypeAsync(LeadType leadType)
        {
            _dbContext.Add(leadType);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<bool> CreateLeadAsync(Lead lead)
        {
            _dbContext.Add(lead);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdateLeadAsync(int leadId)
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<LeadType?> GetLeadTypeAsync(int leadTypeId)
        {
            IQueryable<LeadType> leadTypes = _dbContext.LeadType;
            return await leadTypes.Where(l => l.LeadTypeId == leadTypeId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Lead>> GetLeadsAsync(LeadFilter leadFilter)
        {
            IQueryable<Lead> leads = _dbContext.Leads;

            if (leadFilter.IncludeSource)
                leads = leads.Include(l => l.Source);

            if (leadFilter.IncludeSupplier)
                leads = leads.Include(l => l.Supplier);

            if(leadFilter.IncludeContacts)
                leads = leads.Include(l => l.Contacts)
                             .ThenInclude(c => c.PhoneNumbers)
                             .Include(c => c.Contacts)
                             .ThenInclude(c => c.Addresses);           

            if (leadFilter.supplierIds.Count() > 0)
                leads = leads.Where(l => leadFilter.supplierIds.Contains(l.SupplierId)); 


            return await leads.ToListAsync();
        }

        public async Task<Lead?> GetLeadWithIdAsync(int Id, 
            bool includeSource = false,
            bool includeSupplier = false,
            bool includeContacts = false)
        {
            IQueryable<Lead> leads = _dbContext.Leads; 

            if (includeSource)
                leads = leads.Include(l => l.Source);

            if (includeSupplier)
                leads = leads.Include(l => l.Supplier);

            if (includeContacts)
                leads = leads.Include(l => l.Contacts)
                             .ThenInclude(c => c.PhoneNumbers)
                             .Include(c => c.Contacts)
                             .ThenInclude(c => c.Addresses);

            return await leads.Where(l => l.LeadId == Id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteLead(Lead lead)
        {
           _dbContext.Remove(lead);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        #endregion
              

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }        
    }
}
