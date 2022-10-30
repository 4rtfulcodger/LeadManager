using LeadManager.API.Models;

namespace LeadManager.API
{
    public class TestDataStore
    {
        public List<SourceDto> Sources { get; set; }

        public static TestDataStore Current { get; } = new TestDataStore();


        public TestDataStore()
        {

            Sources = new List<SourceDto>()
            {
                new SourceDto{ Id=1, Name="Search Engine", Description="Leads which originated from a search engine"},
                new SourceDto{ Id=2, Name="Social Media", Description="Leads which originated from social media"}
            };
        }
    }
}
