using LeadManager.Core.Entities;
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

        #region Source
        Task<IEnumerable<Source>> GetSourcesAsync();
        Task<bool> AddSourceAsync(Source source);
        Task<bool> UpdateSourceAsync(int Id);
        Task<Source?> GetSourceWithIdAsync(int Id);
        Task<bool> DeleteSource(Source lead);
        #endregion
        #region Supplier
        Task<bool> AddSupplierAsync(Supplier supplier);
        Task<bool> UpdateSupplierAsync(int Id);
        Task<IEnumerable<Supplier>> GetSuppliersAsync();
        Task<Supplier?> GetSupplierWithIdAsync(int Id);
        Task<bool> DeleteSupplier(Supplier supplier);
        #endregion
        #region Lead
        Task<bool> AddLeadAsync(Lead lead);
        Task<bool> UpdateLeadAsync(int Id);
        Task<IEnumerable<Lead>> GetLeadsAsync(LeadFilter leadFilter);
        Task<Lead?> GetLeadWithIdAsync(int Id, bool includeSource = false, bool includeSupplier = false);
        Task<bool> DeleteLead(Lead lead);
        #endregion
    }
}
