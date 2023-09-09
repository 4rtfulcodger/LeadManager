using LeadManager.Core.Helpers;

namespace LeadManager.Core.Interfaces.Lead
{
    public interface ILeadRepository
    {
        Task<bool> SaveChangesAsync();
               
        Task<bool> CreateLeadAsync(Entities.Lead lead);
        Task<bool> UpdateLeadAsync(int Id);
        Task<IEnumerable<Entities.Lead>> GetLeadsAsync(LeadFilter leadFilter);
        Task<Entities.Lead?> GetLeadWithIdAsync(int Id,
            bool includeSource = false,
            bool includeSupplier = false,
            bool includeContacts = false,
            bool includeLeadAttributes = false
            );
        Task<bool> DeleteLead(Entities.Lead lead);        
    }
}
