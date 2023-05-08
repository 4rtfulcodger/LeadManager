using LeadManager.Core.Entities.Source;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Infrastructure.Data.Repositories
{
    public class SourceRepository : ISourceRepository
    {
        private readonly LeadManagerDbContext _dbContext;

        public SourceRepository(LeadManagerDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

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
    }
}
