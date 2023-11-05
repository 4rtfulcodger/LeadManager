
using LeadManager.Core.Helpers;

namespace LeadManager.Core.Interfaces.Source
{
    public interface ISourceRepository
    {
        Task<IEnumerable<LeadManager.Core.Entities.Source.Source>> GetSourcesAsync();
        Task<PagedList<LeadManager.Core.Entities.Source.Source>> GetSourcesAsync(SourceFilter filter);
        Task<bool> AddSourceAsync(Entities.Source.Source source);
        Task<bool> UpdateSourceAsync(int Id);
        Task<Entities.Source.Source?> GetSourceWithIdAsync(int Id);
        Task<bool> DeleteSource(Entities.Source.Source lead);
    }
}
