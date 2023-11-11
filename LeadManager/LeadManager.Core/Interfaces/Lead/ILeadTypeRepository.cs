using LeadManager.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Interfaces.Lead
{   
    public interface ILeadTypeRepository
    {
        Task<bool> CreateLeadTypeAsync(Entities.LeadType leadType);
        Task<bool> UpdateLeadTypeAsync(int Id);
        Task<Entities.LeadType?> GetLeadTypeAsync(int leadTypeId);
        Task<PagedList<LeadManager.Core.Entities.LeadType>> GetLeadTypesAsync(LeadTypeFilter filter);
        Task<bool> DeleteLeadType(Entities.LeadType leadType);
    }
}
