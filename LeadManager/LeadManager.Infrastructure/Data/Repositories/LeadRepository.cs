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
    public class LeadRepository : ILeadRepository
    {
        private readonly LeadManagerDbContext _dbContext;

        public LeadRepository(LeadManagerDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
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

            if (leadFilter.IncludeAttributes)
                leads = leads.Include(l => l.LeadAttributeValues)
                     .ThenInclude(l => l.Attribute); 

                if (leadFilter.supplierIds.Count() > 0)
                leads = leads.Where(l => leadFilter.supplierIds.Contains(l.SupplierId)); 


            return await leads.ToListAsync();
        }

        public async Task<Lead?> GetLeadWithIdAsync(int Id, 
            bool includeSource = false,
            bool includeSupplier = false,
            bool includeContacts = false,
            bool includeLeadAttributes =false)
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

            if (includeLeadAttributes)
                leads = leads.Include(l => l.LeadAttributeValues)
                    .ThenInclude(l => l.Attribute);

            return await leads.Where(l => l.LeadId == Id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteLead(Lead lead)
        {
           _dbContext.Remove(lead);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }    

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }
    }
}
