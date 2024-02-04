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
    public class LeadTypeRepository : ILeadTypeRepository
    {
        private readonly LeadManagerDbContext _dbContext;

        public LeadTypeRepository(LeadManagerDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateLeadTypeAsync(LeadType leadType)
        {
            _dbContext.Add(leadType);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdateLeadTypeAsync(int Id)
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<PagedList<LeadType>> GetLeadTypesAsync(LeadTypeFilter filter)
        {
            IQueryable<LeadType> filteredLeadTypes = _dbContext.LeadType;
            return await PagedList<LeadType>.Create(filteredLeadTypes, filter.PageNumber, filter.PageSize);
        }

        public async Task<LeadType?> GetLeadTypeAsync(int leadTypeId)
        {
            IQueryable<LeadType> leadTypes = _dbContext.LeadType;
            return await leadTypes.Where(l => l.LeadTypeId == leadTypeId).FirstOrDefaultAsync();
        }              
      
        public async Task<bool> DeleteLeadType(LeadType leadType)
        {
            _dbContext.Remove(leadType);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<LeadType?> GetLeadTypeByReferenceAsync(Guid leadTypeRef)
        {
            IQueryable<LeadType> leadTypes = _dbContext.LeadType;
            return await leadTypes.Where(l => l.LeadTypeReference == leadTypeRef).FirstOrDefaultAsync();
        }
    }
}
