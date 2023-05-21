using LeadManager.Core.Entities;
using LeadManager.Core.Helpers;

namespace LeadManager.Core.Interfaces.Lead
{
    public interface ILeadService
    {
        Task<bool> AddLeadAsync(Entities.Lead.Lead lead);
        Task<bool> UpdateLeadAsync(int Id);
        Task<IEnumerable<Entities.Lead.Lead>> GetLeadsAsync(LeadFilter leadFilter);
        Task<Entities.Lead.Lead?> GetLeadWithIdAsync(int Id, bool includeSource = false, bool includeSupplier = false);
        Task<bool> DeleteLead(LeadManager.Core.Entities.Lead.Lead lead);
    }
}
