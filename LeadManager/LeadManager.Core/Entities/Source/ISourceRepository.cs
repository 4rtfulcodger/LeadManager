using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Entities.Source
{
    public interface ISourceRepository
    {
        Task<IEnumerable<Source>> GetSourcesAsync();
        Task<bool> AddSourceAsync(Source source);
        Task<bool> UpdateSourceAsync(int Id);
        Task<Source?> GetSourceWithIdAsync(int Id);
        Task<bool> DeleteSource(Source lead);
    }
}
