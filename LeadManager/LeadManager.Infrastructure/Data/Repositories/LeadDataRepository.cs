using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Infrastructure.Data.Repositories
{
    public class LeadDataRepository :  ILeadDataRepository
    {
        private List<Source> _sources;
        private List<Supplier> _suppliers;
        private List<Lead> _leads;

        public List<Source> Sources { get => _sources; set => _sources = value; }
        public List<Supplier> Suppliers { get => _suppliers; set => _suppliers = value; }
        public List<Lead> Leads { get => _leads; set => _leads = value; }
        List<Supplier> ILeadDataRepository.GetSuppliers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Supplier GetSupplierWithId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LeadDataRepository()
        {
            _sources = new List<Source>()
            {
                new Source{ Id=1, Name="Search Engine", Description="Leads which originated from a search engine, stored in LeadDataRepository"},
                new Source{ Id=2, Name="Social Media", Description="Leads which originated from social media, stored in LeadDataRepository"}
            };
            _suppliers = new List<Supplier>()
            {
                new Supplier{ Id=1, Name="Supplier1", Description="Lead supplier with name Supplier1 from LeadDataRepository"},
                new Supplier{ Id=2, Name="Supplier2", Description="Lead supplier with name Supplier2 from LeadDataRepository"}
            };
            Leads = new List<Lead>()
            {
                new Lead{ Id=1, Name="Lead1", Description="Lead 1 description from LeadDataRepository"},
                new Lead{ Id=2, Name="Lead2", Description="Lead 2 description from LeadDataRepository"}
            };
        }

        public List<Supplier> GetSuppliers() 
        {
            return Suppliers;
        }

        public List<Lead> GetLeads()
        {
            return Leads;
        }

        public List<Source> GetSources()
        {
            return Sources;
        }

        public Lead GetLeadWithId(int Id)
        {
            return _leads.FirstOrDefault(x => x.Id == Id);
        }

        public Lead GetSourceWithId(int Id)
        {
           return _leads.FirstOrDefault(x => x.Id == Id);
        }

        Source ILeadDataRepository.GetSourceWithId(int Id)
        {
            return _sources.FirstOrDefault(x => x.Id == Id);
        }
    }
}
