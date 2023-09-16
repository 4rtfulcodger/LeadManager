using LeadManager.Core.Entities;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Lead;
using LeadManager.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;


namespace LeadManager.Infrastructure.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILogger<LeadService> _logger;
        public ILeadRepository _leadRepository;
        public ILeadTypeRepository _leadTypeRepository;
        public ILeadAttributeRepository _leadAttributeRepository;

        public LeadService(ILeadRepository sourceRepository,
            ILeadTypeRepository leadTypeRepository,
            ILeadAttributeRepository leadAttributeRepository,
            ILogger<LeadService> logger)
        {
            _leadRepository = sourceRepository ?? throw new ArgumentException(nameof(sourceRepository));
            _leadTypeRepository = leadTypeRepository ?? throw new ArgumentException(nameof(leadTypeRepository));
            _leadAttributeRepository = leadAttributeRepository ?? throw new ArgumentException(nameof(leadAttributeRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<bool> CreateLeadTypeAsync(LeadType leadType)
        {
            leadType.LeadTypeReference = Guid.NewGuid();
            leadType.CreatedOn = DateTime.Now;

            return await _leadTypeRepository.CreateLeadTypeAsync(leadType);
        }

        public async Task<bool> CreateLeadAsync(Lead lead)
        {
            lead.LeadRef = LeadHelper.GenerateLeadReference();
            lead.CreatedDate = DateTime.Now;

            return await _leadRepository.CreateLeadAsync(lead);
        }        

        public async Task<bool> DeleteLeadType(LeadType leadType)
        {
            return await _leadTypeRepository.DeleteLeadType(leadType);
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
            return await _leadTypeRepository.GetLeadTypeAsync(leadTypeId);
        }

        public async Task<Lead?> GetLeadWithIdAsync(int Id,
            bool includeSource = false,
            bool includeSupplier = false,
            bool includeContacts = false,
            bool includeAttributes = false)
        {
            return await _leadRepository.GetLeadWithIdAsync(Id,
                includeSource,
                includeSupplier,
                includeContacts,
                includeAttributes);
        }

        public async Task<bool> UpdateLeadAsync(int Id)
        {
            return await _leadRepository.UpdateLeadAsync(Id);
        }

        public async Task<bool> UpdateLeadTypeAsync(int Id)
        {
            return await _leadTypeRepository.UpdateLeadTypeAsync(Id);
        }

        public async Task<bool> CreateLeadAttributeAsync(LeadAttribute leadAttribute)
        {
            return await _leadAttributeRepository.CreateLeadAttributeAsync(leadAttribute);
        }

        public async Task<bool> UpdateLeadAttributeAsync(int Id)
        {
            return await _leadAttributeRepository.UpdateLeadAttributeAsync(Id);
        }

        public async Task<LeadAttribute?> GetLeadAttributeAsync(int leadAttributeId)
        {
            return await _leadAttributeRepository.GetLeadAttributeAsync(leadAttributeId);
        }

        public async Task<IEnumerable<LeadAttribute>?> GetLeadAttributesAsync(int leadTypeId)
        {
            return await _leadAttributeRepository.GetLeadAttributesAsync(leadTypeId);
        }

        public async Task<bool> DeleteLeadAttribute(LeadAttribute leadAttribute)
        {
            return await _leadAttributeRepository.DeleteLeadAttribute(leadAttribute);
        }
    }
}
