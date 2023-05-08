using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Interfaces
{
    public interface ILeadInfoRepository
    {
        Task<bool> SaveChangesAsync();

        #region Lead
        Task<bool> AddLeadAsync(Lead lead);
        Task<bool> UpdateLeadAsync(int Id);
        Task<IEnumerable<Lead>> GetLeadsAsync(LeadFilter leadFilter);
        Task<Lead?> GetLeadWithIdAsync(int Id, bool includeSource = false, bool includeSupplier = false);
        Task<bool> DeleteLead(Lead lead);
        #endregion
    }
}
