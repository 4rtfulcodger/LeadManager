using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Interfaces.Lead
{
    public interface ILeadAttributeRepository
    {
        Task<bool> CreateLeadAttributeAsync(Entities.LeadAttribute leadAttribute);
        Task<bool> UpdateLeadAttributeAsync(int Id);
        Task<Entities.LeadAttribute?> GetLeadAttributeAsync(int leadAttributeId);
        Task<IEnumerable<Entities.LeadAttribute>?> GetLeadAttributesAsync(int leadTypeId);
        Task<bool> DeleteLeadAttribute(Entities.LeadAttribute leadAttribute);
    }
}
