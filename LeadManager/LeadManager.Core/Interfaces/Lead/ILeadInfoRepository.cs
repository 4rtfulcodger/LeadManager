using LeadManager.Core.Helpers;

namespace LeadManager.Core.Interfaces.Lead
{
    public interface ILeadInfoRepository
    {
        Task<bool> SaveChangesAsync();

        #region Lead
        Task<bool> AddLeadAsync(Entities.Lead lead);
        Task<bool> UpdateLeadAsync(int Id);
        Task<IEnumerable<Entities.Lead>> GetLeadsAsync(LeadFilter leadFilter);
        Task<Entities.Lead?> GetLeadWithIdAsync(int Id, bool includeSource = false, bool includeSupplier = false);
        Task<bool> DeleteLead(Entities.Lead lead);
        #endregion
    }
}
