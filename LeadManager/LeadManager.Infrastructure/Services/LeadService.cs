using LeadManager.Core.Entities;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Lead;
using Microsoft.Extensions.Logging;


namespace LeadManager.Infrastructure.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILogger<LeadService> _logger;
        public ILeadInfoRepository _leadRepository;

        public LeadService(ILeadInfoRepository sourceRepository,
            ILogger<LeadService> logger)
        {
            _leadRepository = sourceRepository ?? throw new ArgumentException(nameof(sourceRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<bool> AddLeadAsync(Lead lead)
        {
            lead.LeadRef = LeadHelper.GenerateLeadReference();

            return await _leadRepository.CreateLeadAsync(lead);
        }

        public async Task<bool> CreateLeadTypeAsync(LeadType leadType)
        {
            leadType.LeadTypeReference = Guid.NewGuid();
            leadType.CreatedOn = DateTime.Now;

            return await _leadRepository.CreateLeadTypeAsync(leadType);
        }

        public async Task<bool> DeleteLead(Lead lead)
        {
            return await _leadRepository.DeleteLead(lead);
        }

        public async Task<IEnumerable<Lead>> GetLeadsAsync(LeadFilter leadFilter)
        {
            return await _leadRepository.GetLeadsAsync(leadFilter);
        }

        public async Task<LeadType?> GetLeadTypeAsync(int leadTypeId)
        {
            return await _leadRepository.GetLeadTypeAsync(leadTypeId);
        }

        public async Task<Lead?> GetLeadWithIdAsync(int Id,
            bool includeSource = false,
            bool includeSupplier = false,
            bool includeContacts = false)
        {
            return await _leadRepository.GetLeadWithIdAsync(Id,
                includeSource,
                includeSupplier,
                includeContacts);
        }

        public async Task<bool> UpdateLeadAsync(int Id)
        {
            return await _leadRepository.UpdateLeadAsync(Id);
        }
    }
}
