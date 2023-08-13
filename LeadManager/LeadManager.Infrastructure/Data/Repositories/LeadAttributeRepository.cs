using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces.Lead;
using Microsoft.EntityFrameworkCore;

namespace LeadManager.Infrastructure.Data.Repositories
{
    public class LeadAttributeRepository : ILeadAttributeRepository
    {
        private readonly LeadManagerDbContext _dbContext;
        public LeadAttributeRepository(LeadManagerDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateLeadAttributeAsync(LeadAttribute leadAttribute)
        {
            _dbContext.Add(leadAttribute);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<bool> DeleteLeadAttribute(LeadAttribute leadAttribute)
        {
            _dbContext.Remove(leadAttribute);
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        public async Task<LeadAttribute?> GetLeadAttributeAsync(int leadAttributeId)
        {
            IQueryable<LeadAttribute> leadAttributes = _dbContext.LeadAttribute;
            return await leadAttributes.Where(l => l.LeadAttributeId == leadAttributeId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<LeadAttribute>?> GetLeadAttributesAsync(int leadTypeId)
        {
            IQueryable<LeadAttribute> leadAttributes = _dbContext.LeadAttribute;
            leadAttributes = leadAttributes.Include(la => la.LeadType);
            return await leadAttributes.Where(la => la.LeadType.LeadTypeId == leadTypeId).ToListAsync();
        }

        public async Task<bool> UpdateLeadAttributeAsync(int Id)
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }
    }
}
