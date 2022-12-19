using LeadManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Interfaces
{
    public interface ILeadDataRepository
    {

        List<Lead> GetLeads();
        Lead GetLeadWithId(int Id);

        List<Source> GetSources();
        Source GetSourceWithId(int Id);

        List<Supplier> GetSuppliers { get; set; }
        Supplier GetSupplierWithId { get; set; }
    }
}
