
using LeadManager.Core.Helpers;

namespace LeadManager.Core.Interfaces.Source
{
    public interface ISourceService
    {
        Task<PagedList<LeadManager.Core.Entities.Source.Source>> GetSourcesAsync(SourceFilter filter);
        Task<LeadManager.Core.Entities.Source.Source> GetSourceWithIdAsync(int sourceId);
        Task<LeadManager.Core.Entities.Source.Source> GetSourceWithRefAsync(Guid sourceRef);
        Task<bool> CreateSourceAsync(Entities.Source.Source source);
        Task<bool> UpdateSourceAsync(int sourceId);
        Task<bool> DeleteSource(Entities.Source.Source source);
    }
}
