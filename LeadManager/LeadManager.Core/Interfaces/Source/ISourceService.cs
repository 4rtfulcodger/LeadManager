
namespace LeadManager.Core.Interfaces.Source
{
    public interface ISourceService
    {
        //Need to add a filter parameter
        Task<IEnumerable<Entities.Source.Source>> GetSourcesAsync();
        Task<LeadManager.Core.Entities.Source.Source> GetSourceWithIdAsync(int sourceId);
        Task<bool> CreateSourceAsync(Entities.Source.Source source);
        Task<bool> UpdateSourceAsync(int sourceId);
        Task<bool> DeleteSource(Entities.Source.Source source);
    }
}
