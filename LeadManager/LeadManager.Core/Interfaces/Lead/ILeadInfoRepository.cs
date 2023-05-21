using LeadManager.Core.Helpers;

namespace LeadManager.Core.Interfaces.Lead
{
    public interface ILeadInfoRepository
    {
        Task<bool> SaveChangesAsync();

        #region Lead
        Task<bool> AddLeadAsync(Entities.Lead.Lead lead);
        Task<bool> UpdateLeadAsync(int Id);
        Task<IEnumerable<Entities.Lead.Lead>> GetLeadsAsync(LeadFilter leadFilter);
        Task<Entities.Lead.Lead?> GetLeadWithIdAsync(int Id, bool includeSource = false, bool includeSupplier = false);
        Task<bool> DeleteLead(Entities.Lead.Lead lead);
        #endregion
    }
}
