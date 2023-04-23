using LeadManager.Core.Entities.Source;
using LeadManager.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeadManager.Infrastructure.Services 
{
    public class SourceService : ISourceService
    {
        
        private readonly ILogger<SourceService> _logger;
        public ILeadInfoRepository _leadInfoRepository;

        public SourceService(ILeadInfoRepository leadInfoRepository,
            ILogger<SourceService> logger)
        {
            _leadInfoRepository = leadInfoRepository ?? throw new ArgumentException(nameof(leadInfoRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<bool> CreateSourceAsync(Source sourceDto)
        {
           return await _leadInfoRepository.AddSourceAsync(sourceDto);
        }

        //Need to add a filter parameter
        public async Task<IEnumerable<Source>> GetSourcesAsync()
        {            
            return await _leadInfoRepository.GetSourcesAsync();
        }

        public async Task<Source> GetSourceWithIdAsync(int id)
        {
            return await _leadInfoRepository.GetSourceWithIdAsync(id);
            
        }

        public async Task<bool> UpdateSourceAsync(int sourceId)
        {
           return await _leadInfoRepository.UpdateSourceAsync(sourceId);
        }

        public async Task<bool> DeleteSource(Source source)
        {
           return await _leadInfoRepository.DeleteSource(source);
        }
    }
}
