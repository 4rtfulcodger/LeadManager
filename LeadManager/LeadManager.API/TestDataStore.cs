using LeadManager.API.Models;

namespace LeadManager.API
{
    public class TestDataStore 
    {
        public List<SourceDto> Sources { get; set; }
        public List<SupplierDto> Suppliers { get; set; }
        public List<LeadDto> Leads { get; set; }

        public static TestDataStore Current { get; } = new TestDataStore();


        public TestDataStore()
        {

            Sources = new List<SourceDto>()
            {
                new SourceDto{ Id=1, Name="Search Engine", Description="Leads which originated from a search engine"},
                new SourceDto{ Id=2, Name="Social Media", Description="Leads which originated from social media"}
            };
            Suppliers = new List<SupplierDto>()
            {
                new SupplierDto{ Id=1, Name="Supplier1", Description="Lead supplier with name Supplier1"},
                new SupplierDto{ Id=2, Name="Supplier2", Description="Lead supplier with name Supplier1"}
            };
            Leads = new List<LeadDto>()
            {
                new LeadDto{ Id=1, Name="Lead1", Description="Lead 1 description"},
                new LeadDto{ Id=2, Name="Lead2", Description="Lead 2 description"}
            };
        }
    }
}
