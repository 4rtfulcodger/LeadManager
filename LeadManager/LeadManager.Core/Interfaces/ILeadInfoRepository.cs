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
        Task<IEnumerable<Source>> GetSourcesAsync();        
        Task<Source?> GetSourceWithIdAsync(int Id);

        Task<IEnumerable<Supplier>> GetSuppliersAsync();
        Task<Supplier?> GetSupplierWithIdAsync(int Id);

        Task<IEnumerable<Lead>> GetLeadsAsync();
        Task<Lead?> GetLeadWithIdAsync(int Id, bool includeSource = false, bool includeSupplier = false);
    }
}
