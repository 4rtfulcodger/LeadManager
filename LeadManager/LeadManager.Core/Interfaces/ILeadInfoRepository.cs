using LeadManager.Core.Entities;
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

        #region Source
        Task<IEnumerable<Source>> GetSourcesAsync();        
        Task<Source?> GetSourceWithIdAsync(int Id);
        #endregion
        #region
        Task<IEnumerable<Supplier>> GetSuppliersAsync();
        Task<Supplier?> GetSupplierWithIdAsync(int Id);
        #endregion
        #region Lead
        Task<bool> AddLeadAsync(Lead lead);
        Task<IEnumerable<Lead>> GetLeadsAsync(bool includeSource = false, bool includeSupplier = false);
        Task<Lead?> GetLeadWithIdAsync(int Id, bool includeSource = false, bool includeSupplier = false);
        #endregion
    }
}
