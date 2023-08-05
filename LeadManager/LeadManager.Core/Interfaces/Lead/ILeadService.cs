using LeadManager.Core.Entities;
using LeadManager.Core.Helpers;

namespace LeadManager.Core.Interfaces.Lead
{
    public interface ILeadService
    {
        Task<bool> CreateLeadTypeAsync(Entities.LeadType leadType);
        Task<bool> AddLeadAsync(Entities.Lead lead);
        Task<bool> UpdateLeadAsync(int Id);
        Task<Entities.LeadType?> GetLeadTypeAsync(int leadTypeId);
        Task<IEnumerable<Entities.Lead>> GetLeadsAsync(LeadFilter leadFilter);
        Task<Entities.Lead?> GetLeadWithIdAsync(int Id,
            bool includeSource = false,
            bool includeSupplier = false,
            bool includeContacts = false);
        Task<bool> DeleteLead(Entities.Lead lead);        
    }
}
