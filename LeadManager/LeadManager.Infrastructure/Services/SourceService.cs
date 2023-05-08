using LeadManager.Core.Entities.Source;
using LeadManager.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeadManager.Infrastructure.Services 
{
    public class SourceService : ISourceService
    {
        
        private readonly ILogger<SourceService> _logger;
        public ISourceRepository _sourceRepository;

        public SourceService(ISourceRepository sourceRepository,
            ILogger<SourceService> logger)
        {
            _sourceRepository = sourceRepository ?? throw new ArgumentException(nameof(sourceRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<bool> CreateSourceAsync(Source sourceDto)
        {
           return await _sourceRepository.AddSourceAsync(sourceDto);
        }

        //Need to add a filter parameter
        public async Task<IEnumerable<Source>> GetSourcesAsync()
        {            
            return await _sourceRepository.GetSourcesAsync();
        }

        public async Task<Source> GetSourceWithIdAsync(int id)
        {
            return await _sourceRepository.GetSourceWithIdAsync(id);
            
        }

        public async Task<bool> UpdateSourceAsync(int sourceId)
        {
           return await _sourceRepository.UpdateSourceAsync(sourceId);
        }

        public async Task<bool> DeleteSource(Source source)
        {
           return await _sourceRepository.DeleteSource(source);
        }
    }
}
