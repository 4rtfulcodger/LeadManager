using LeadManager.Core.Helpers;

namespace LeadManager.Core.Interfaces.Lead
{
    public interface ILeadInfoRepository
    {
        Task<bool> SaveChangesAsync();

        #region Lead
        Task<bool> CreateLeadAsync(Entities.Lead lead);
        Task<bool> CreateLeadTypeAsync(Entities.LeadType leadType);
        Task<bool> UpdateLeadAsync(int Id);
        Task<IEnumerable<Entities.Lead>> GetLeadsAsync(LeadFilter leadFilter);

        Task<Entities.LeadType?> GetLeadTypeAsync(int leadTypeId);

        Task<Entities.Lead?> GetLeadWithIdAsync(int Id,
            bool includeSource = false,
            bool includeSupplier = false,
            bool includeContacts = false
            );
        Task<bool> DeleteLead(Entities.Lead lead);
        #endregion
    }
}
