using LeadManager.Core.Entities;
using LeadManager.Core.Helpers;

namespace LeadManager.Core.Interfaces.Lead
{
    public interface ILeadService
    {
        Task<bool> CreateLeadTypeAsync(Entities.LeadType leadType);
        Task<bool> CreateLeadAsync(Entities.Lead lead);
        Task<bool> UpdateLeadTypeAsync(int Id);
        Task<bool> UpdateLeadAsync(int Id);        
        Task<Entities.LeadType?> GetLeadTypeAsync(int leadTypeId);
        Task<IEnumerable<Entities.Lead>> GetLeadsAsync(LeadFilter leadFilter);
        Task<Entities.Lead?> GetLeadWithIdAsync(int Id,
            bool includeSource = false,
            bool includeSupplier = false,
            bool includeContacts = false);
        Task<bool> DeleteLeadType(Entities.LeadType leadType);
        Task<bool> DeleteLead(Entities.Lead lead);

        Task<bool> CreateLeadAttributeAsync(Entities.LeadAttribute leadAttribute);
        Task<bool> UpdateLeadAttributeAsync(int Id);
        Task<Entities.LeadAttribute?> GetLeadAttributeAsync(int leadAttributeId);
        Task<IEnumerable<Entities.LeadAttribute>?> GetLeadAttributesAsync(int leadTypeId);
        Task<bool> DeleteLeadAttribute(Entities.LeadAttribute leadAttribute);
    }
}
