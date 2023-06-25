using LeadManager.Core.Entities.Lead;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Lead;
using Microsoft.Extensions.Logging;


namespace LeadManager.Infrastructure.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILogger<LeadService> _logger;
        public ILeadInfoRepository _sourceRepository;

        public LeadService(ILeadInfoRepository sourceRepository,
            ILogger<LeadService> logger)
        {
            _sourceRepository = sourceRepository ?? throw new ArgumentException(nameof(sourceRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<bool> AddLeadAsync(Lead lead)
        {
            lead.LeadRef = LeadHelper.GenerateLeadReference();

            return await _sourceRepository.AddLeadAsync(lead);
        }

        public async Task<bool> DeleteLead(Lead lead)
        {
            return await _sourceRepository.DeleteLead(lead);
        }

        public async Task<IEnumerable<Lead>> GetLeadsAsync(LeadFilter leadFilter)
        {
            return await _sourceRepository.GetLeadsAsync(leadFilter);
        }

        public async Task<Lead?> GetLeadWithIdAsync(int Id, bool includeSource = false, bool includeSupplier = false)
        {
            return await _sourceRepository.GetLeadWithIdAsync(Id,includeSource, includeSupplier);
        }

        public async Task<bool> UpdateLeadAsync(int Id)
        {
            return await _sourceRepository.UpdateLeadAsync(Id);
        }
    }
}
