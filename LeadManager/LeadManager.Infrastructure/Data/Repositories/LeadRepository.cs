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

        public async Task<PagedList<Lead>> GetLeadsAsync(LeadFilter leadFilter)
        {
            IQueryable<Lead> filteredLeads = _dbContext.Leads;

            if (leadFilter.IncludeSource)
                filteredLeads = filteredLeads.Include(l => l.Source);

            if (leadFilter.IncludeSupplier)
                filteredLeads = filteredLeads.Include(l => l.Supplier);

            if(leadFilter.IncludeContacts)
                filteredLeads = filteredLeads.Include(l => l.Contacts)
                             .ThenInclude(c => c.PhoneNumbers)
                             .Include(c => c.Contacts)
                             .ThenInclude(c => c.Addresses);

            if (leadFilter.IncludeAttributes)
                filteredLeads = filteredLeads.Include(l => l.LeadAttributeValues)
                     .ThenInclude(l => l.Attribute);

            //If filter contains a list of lead Ids, search for filteredLeads containing those lead Ids
            //Else, search using CreatedDate
            if (leadFilter.supplierIds.Count() > 0)
            {
                filteredLeads = filteredLeads.Where(l => leadFilter.supplierIds.Contains(l.SupplierId));
            }
            else
            {
                filteredLeads = filteredLeads.Where(ld => ld.CreatedDate >= leadFilter.FromCreatedDate)
                                               .Where(ld => ld.CreatedDate <= leadFilter.ToCreatedDate);
            }

            return await PagedList<Lead>.Create(filteredLeads, leadFilter.PageNumber, leadFilter.PageSize); 
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
